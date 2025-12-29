using IPE.SmsIrClient;
using IPE.SmsIrClient.Exceptions;
using IPE.SmsIrClient.Models.Requests;
using IPE.SmsIrClient.Models.Results;

namespace Servino.Infra.Providers.SmsProvider.SmsIrService;

public class SmsIrService(string apiKey) : ISmsService
{
    private readonly SmsIr smsIr = new(apiKey);

    public async Task<VerifySendResult> SendOtpAsync(string mobileNumber, int templateId, List<VerifySendParameter> parameters)
    {
        try
        {
            // دریافت پاسخ - نوع response یک wrapper با پراپرتی Data هست
            var response = await smsIr.VerifySendAsync(
                mobile: mobileNumber,
                templateId: templateId,
                parameters: parameters.ToArray()
            );

            // اگر به اینجا رسید یعنی ارسال موفق بوده (کتابخانه در خطا exception می‌ندازه)
            return response.Data; // مستقیم VerifySendResult رو برمی‌گردونیم
        }
        catch (Exception ex)
        {
            // مدیریت خطاهای شناخته‌شده (دقیقاً مثل کد نمونه شما)
            string friendlyMessage = ex switch
            {
                UnauthorizedException => "کلید API نامعتبر یا منقضی شده است.",
                LogicalException lex => $"پارامترهای ارسالی اشتباه است: {lex.Message}",
                TooManyRequestException => "تعداد درخواست‌ها بیش از حد مجاز است. کمی صبر کنید.",
                UnexpectedException => "خطای غیرمنتظره از سمت سرور Sms.ir.",
                InvalidOperationException => "عملیات نامعتبر (مثل قالب تأیید نشده).",
                _ => $"خطای ناشناخته: {ex.Message}"
            };

            throw new Exception($"ارسال OTP شکست خورد: {friendlyMessage}", ex);
        }
    }
}