using app.Application.Contracts.Models;

namespace app.Application.Contracts.Contracts.Services;

public interface IEmailSender
{
    Task<bool> SendEmail(Email email);
}