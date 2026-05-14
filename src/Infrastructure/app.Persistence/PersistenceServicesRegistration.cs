using app.Application.Contracts.Contracts.Repositories;
using app.Infrastructure.Identity.Service;
using app.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace app.Persistence;

public static class PersistenceServicesRegistration
{
    public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ServinoConnectionString")));

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IHomeServiceRepository, HomeServiceRepository>();
        services.AddScoped<IExpertRepository, ExpertRepository>();
        services.AddScoped<IExpertHomeServiceRepository, ExpertHomeServiceRepository>();
        services.AddScoped<IProvinceRepository, ProvinceRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IRequestRepository, RequestRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ISuggestionRepository, SuggestionRepository>();
        services.AddScoped<IAdminRepository, AdminRepository>();



        services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 4;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddClaimsPrincipalFactory<AppUserClaimsPrincipalFactory>();

        return services;
    }
}