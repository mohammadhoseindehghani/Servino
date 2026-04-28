using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace app.Application;

public static class ApplicationServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(ApplicationServiceRegistration).Assembly);
    }
}
