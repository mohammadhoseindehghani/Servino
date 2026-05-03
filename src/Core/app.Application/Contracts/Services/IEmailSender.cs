using app.Application.Models;

namespace app.Application.Contracts.Services;

public interface IEmailSender
{
    Task<bool> SendEmail(Email email);
}