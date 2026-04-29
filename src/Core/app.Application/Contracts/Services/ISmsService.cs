using IPE.SmsIrClient.Models.Requests;
using IPE.SmsIrClient.Models.Results;

namespace app.Application.Contracts.Services;

public interface ISmsService
{
    Task<VerifySendResult> SendOtpAsync(string mobileNumber, int templateId, List<VerifySendParameter> parameters);
    Task<VerifySendResult> SendByTemplateAsync(string mobileNumber, int templateId, List<VerifySendParameter> parameters, CancellationToken ct = default);
}