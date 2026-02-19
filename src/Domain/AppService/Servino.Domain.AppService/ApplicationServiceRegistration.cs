using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Servino.Domain.AppService.UserAgg;
using Servino.Domain.Core.CategoryAgg.Contracts.AppService;
using Servino.Domain.Core.CommentAgg.Contracts.AppService;
using Servino.Domain.Core.HomeServiceAgg.Contracts.AppService;
using Servino.Domain.Core.LocationAgg.Contracts.AppService;
using Servino.Domain.Core.RequestAgg.Contracts.AppService;
using Servino.Domain.Core.SuggestionAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Contracts.AppService;

namespace Servino.Domain.AppService;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserAppService, UserAppService>();
        services.AddScoped<ICommentAppService, CommentAppService>();
        services.AddScoped<ICategoryAppService, CategoryAppService>();
        services.AddScoped<IHomeServiceAppService, HomeServiceAppService>();
        services.AddScoped<IExpertAppService, ExpertAppService>();
        services.AddScoped<IProvinceAppService, ProvinceAppService>();
        services.AddScoped<ICityAppService, CityAppService>();
        services.AddScoped<IRequestAppService, RequestAppService>();
        services.AddScoped<ICustomerAppService, CustomerAppService>();
        services.AddScoped<ISuggestionAppService, SuggestionAppService>();
        services.AddScoped<IAdminAppService, AdminAppService>();

        return services;
    }
}