using Hangfire;
using IPE.SmsIrClient.Models.Requests;
using Microsoft.Extensions.Logging;
using Servino.Domain.Core.RequestAgg.Contracts.Service;
using Servino.Infra.Providers.SmsProvider.SmsIrService;

namespace Servino.Infrastructure.BackgroundJobs;

public class RequestReminderJob(
    IRequestService requestService,
    ISmsService smsService,
    ILogger<RequestReminderJob> logger)
{
    private const int HoursThreshold = 3;

    [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
    public async Task CheckRequestsWithoutSuggestion(IJobCancellationToken token)
    {
        var ct = token.ShutdownToken;

        logger.LogInformation("RequestReminderJob started. Threshold={Hours}h", HoursThreshold);

        var requests = await requestService.GetRequestsWithoutSuggestionAsync(ct);

        if (requests.Count == 0)
        {
            logger.LogInformation("RequestReminderJob finished. No pending requests without suggestions.");
            return;
        }

        logger.LogInformation("RequestReminderJob found {Count} requests without suggestion.", requests.Count);

        foreach (var req in requests)
        {
            ct.ThrowIfCancellationRequested();

            try
            {
                var phone = await requestService.GetCustomerMobileByRequestId(req.Id, ct);
                if (string.IsNullOrWhiteSpace(phone))
                {
                    logger.LogWarning("Request {RequestId}: customer phone is empty. Skipping SMS.", req.Id);
                }
                else
                {
                    const int reminderTemplateId = 688143; 

                    var parameters = new List<VerifySendParameter>
                    {
                        new("title", req.Title),
                        new("requestId", req.Id.ToString())
                    };

                    await smsService.SendOtpAsync(phone, reminderTemplateId, parameters);

                    logger.LogInformation("Request {RequestId}: SMS reminder sent to {Phone}.", req.Id, phone);
                }


                var marked = await requestService.MarkNoSuggestionReminderSentAsync(req.Id, DateTime.UtcNow, ct);

                if (!marked)
                    logger.LogWarning("Request {RequestId}: could not mark reminder sent.", req.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "RequestReminderJob error for RequestId={RequestId}", req.Id);
            }
        }

        logger.LogInformation("RequestReminderJob finished.");
    }
}