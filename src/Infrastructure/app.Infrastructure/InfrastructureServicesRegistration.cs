using app.Application.Contracts.Services;
using app.Application.Models;
using app.Infrastructure.Cache;
using app.Infrastructure.File;
using app.Infrastructure.Mail;
using app.Infrastructure.Sms;
using Microsoft.Extensions.Configuration;
using app.Infrastructure.Identity.Service;
using Microsoft.Extensions.DependencyInjection;

namespace app.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddScoped<IIdentityService, IdentityService>();

        services.AddScoped<ISmsService>(provider =>
            new SmsIrService("Hsglr29hTKTzz9k4F9mDFVdFQMkllYkxv3VV5wDBilPyljbp"));

        services.AddMemoryCache();

        services.AddScoped<ICacheService, RedisCacheService>();

        services.Configure<RedisSettings>(configuration.GetSection("Redis"));

        services.AddStackExchangeRedisCache(options =>
        {
            var redis = configuration.Get<RedisSettings>();
            options.Configuration = redis.Configuration;
            options.InstanceName = redis.InstanceName;
        });



        services.Configure<EmailSetting>(configuration.GetSection("EmailSettings"));
        services.AddScoped<IEmailSender, EmailSender>();

        services.AddScoped<IFileService, FileService>();

        return services;


    }
    
}