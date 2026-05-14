using app.Application.Contracts.Contracts.Services.CategoryAgg;
using app.Application.Contracts.Contracts.Services.CityAgg;
using app.Application.Contracts.Contracts.Services.CommentAgg;
using app.Application.Contracts.Contracts.Services.ExpertHomeServiceAgg;
using app.Application.Contracts.Contracts.Services.HomeServiceAgg;
using app.Application.Contracts.Contracts.Services.ProvinceAgg;
using app.Application.Contracts.Contracts.Services.RequestAgg;
using app.Application.Contracts.Contracts.Services.Suggestion;
using app.Application.Contracts.Contracts.Services.UserAgg;
using app.Application.Features.Admins.Services;
using app.Application.Features.Categories.Services;
using app.Application.Features.City.Services;
using app.Application.Features.Comments.Services;
using app.Application.Features.Customers.Services;
using app.Application.Features.Experts.Services;
using app.Application.Features.HomeServices.Services;
using app.Application.Features.Province.Services;
using app.Application.Features.Requests.Services;
using app.Application.Features.Suggestions.Services;
using app.Application.Features.Users.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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


        // Services
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