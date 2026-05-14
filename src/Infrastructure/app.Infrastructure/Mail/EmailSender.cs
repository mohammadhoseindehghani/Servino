using System.Net;
using app.Application.Contracts.Contracts.Services;
using app.Application.Contracts.Models;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace app.Infrastructure.Mail;

public class EmailSender(IOptions<EmailSetting> options) : IEmailSender
{
    public async Task<bool> SendEmail(Email email)
    {
        var client = new SendGridClient(options.Value.ApiKey);
        var to = new EmailAddress(email.To);
        var from = new EmailAddress()
        {
            Email = options.Value.FromAddress,
            Name = options.Value.FromName
        };
        var message = MailHelper.CreateSingleEmail(from, to, email.Subject, email.Body, email.Body);
        var response = await client.SendEmailAsync(message);
        return response.StatusCode is HttpStatusCode.OK or HttpStatusCode.Accepted;

    }
}