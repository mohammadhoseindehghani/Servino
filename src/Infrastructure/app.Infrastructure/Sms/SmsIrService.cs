using app.Application.Contracts.Contracts.Services;
using IPE.SmsIrClient;
using IPE.SmsIrClient.Exceptions;
using IPE.SmsIrClient.Models.Requests;
using IPE.SmsIrClient.Models.Results;

namespace app.Infrastructure.Sms;

public class SmsIrService(string apiKey) : ISmsService
{
    private readonly SmsIr smsIr = new(apiKey);

    public async Task<VerifySendResult> SendOtpAsync(string mobileNumber, int templateId, List<VerifySendParameter> parameters)
    {
        try
        {
            var response = await smsIr.VerifySendAsync(
                mobile: mobileNumber,
                templateId: templateId,
                parameters: parameters.ToArray()
            );

            return response.Data; 
        }
        catch (Exception ex)
        {
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

    public async Task<VerifySendResult> SendByTemplateAsync(string mobileNumber, int templateId, List<VerifySendParameter> parameters, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();
            var response = await smsIr.VerifySendAsync(
                mobile: mobileNumber,
                templateId: templateId,
                parameters: parameters.ToArray()
            );

            return response.Data;
        }
        catch (Exception ex)
        {
            string friendlyMessage = ex switch
            {
                UnauthorizedException => "کلید API نامعتبر یا منقضی شده است.",
                LogicalException lex => $"پارامترهای ارسالی اشتباه است: {lex.Message}",
                TooManyRequestException => "تعداد درخواست‌ها بیش از حد مجاز است. کمی صبر کنید.",
                UnexpectedException => "خطای غیرمنتظره از سمت سرور Sms.ir.",
                InvalidOperationException => "عملیات نامعتبر (مثل قالب تأیید نشده).",
                _ => $"خطای ناشناخته: {ex.Message}"
            };

            throw new Exception($"ارسال پیام قالبی شکست خورد: {friendlyMessage}", ex);
        }
    }
}