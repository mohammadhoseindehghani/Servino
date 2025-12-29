using IPE.SmsIrClient.Models.Requests;
using IPE.SmsIrClient.Models.Results;

namespace Servino.Infra.Providers.SmsProvider.SmsIrService;

public interface ISmsService
{
    Task<VerifySendResult> SendOtpAsync(string mobileNumber, int templateId, List<VerifySendParameter> parameters);
}