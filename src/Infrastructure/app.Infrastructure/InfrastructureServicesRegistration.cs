using app.Application.Contracts.Services;
using app.Application.Models;
using app.Infrastructure.Mail;
using app.Infrastructure.Sms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace app.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection ConfigureInfrastructureServices(this ServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddScoped<IIdentityService, IIdentityService>();
        services.AddScoped<ISmsService, SmsIrService>();

        services.Configure<EmailSetting>(configuration.GetSection("EmailSettings"));
        services.AddScoped<IEmailSender, EmailSender>();


    }
    
}