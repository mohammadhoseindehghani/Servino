using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace app.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        //Automapper
        services.AddAutoMapper(cfg => { }, typeof(ApplicationServiceRegistration).Assembly);

        //Mediator
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
        });

        // FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}