using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Servino.Domain.Core.CategoryAgg.Contracts.Service;
using Servino.Domain.Core.CommentAgg.Contracts.Service;
using Servino.Domain.Core.ExpertHomeServiceAgg.Contracts.Service;
using Servino.Domain.Core.HomeServiceAgg.Contracts.Service;
using Servino.Domain.Core.LocationAgg.Contracts.Service;
using Servino.Domain.Core.RequestAgg.Contracts.Service;
using Servino.Domain.Core.SuggestionAgg.Contracts.Service;
using Servino.Domain.Core.UserAgg.Contracts.Service;

namespace Servino.Domain.Service;

public static class ServiceServiceRegistration
{
    public static IServiceCollection AddServiceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IHomeServiceService, HomeServiceService>();
        services.AddScoped<IExpertHomeServiceService, ExpertHomeServiceService>();
        services.AddScoped<IExpertService, ExpertService>();
        services.AddScoped<IProvinceService, ProvinceService>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<IRequestService, RequestService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ISuggestionService, SuggestionService>();
        services.AddScoped<IAdminService, AdminService>();

        return services;
    }
}