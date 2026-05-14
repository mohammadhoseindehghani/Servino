using app.Application.Contracts.Models;

namespace app.Application.Contracts.Contracts.Providers_Services;

public interface IEmailSender
{
    Task<bool> SendEmail(Email email);
}