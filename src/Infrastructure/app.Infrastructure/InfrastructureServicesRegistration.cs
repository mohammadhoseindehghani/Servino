using app.Infrastructure.Cache;
using app.Infrastructure.File;
using app.Infrastructure.Identity.Service;
using app.Infrastructure.Mail;
using app.Infrastructure.Sms;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using app.Application.Contracts.Contracts.Services;
using app.Application.Contracts.Models;

namespace app.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection ConfigureInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        // Identity
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, AppUserClaimsPrincipalFactory>();


        // Jwt Settings
        var jwtSettings = configuration
            .GetSection("JwtSettings")
            .Get<JwtSettings>()
            ?? throw new InvalidOperationException("JwtSettings not found");

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.AddScoped<IJwtTokenService, JwtTokenService>();


        var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);

        services
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,

                IssuerSigningKey = new SymmetricSecurityKey(key),

                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization();


        // SMS
        services.AddScoped<ISmsService>(provider =>
            new SmsIrService("Hsglr29hTKTzz9k4F9mDFVdFQMkllYkxv3VV5wDBilPyljbp"));

        services.AddMemoryCache();


        // Redis
        services.Configure<RedisSettings>(configuration.GetSection("Redis"));

        services.AddStackExchangeRedisCache(options =>
        {
            var redis = configuration.GetSection("Redis").Get<RedisSettings>();

            if (redis is null)
                throw new InvalidOperationException("Redis configuration section 'Redis' is missing");

            options.Configuration = redis.Configuration;
            options.InstanceName = redis.InstanceName;
        });

        services.AddScoped<ICacheService, RedisCacheService>();


        // Email
        services.Configure<EmailSetting>(configuration.GetSection("EmailSettings"));
        services.AddScoped<IEmailSender, EmailSender>();


        // File
        services.AddScoped<IFileService, FileService>();


        return services;
    }
}
