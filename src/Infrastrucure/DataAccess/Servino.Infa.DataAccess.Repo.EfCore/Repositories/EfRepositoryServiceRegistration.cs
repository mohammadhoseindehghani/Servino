using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Servino.Domain.Core.CategoryAgg.Contracts.Data;
using Servino.Domain.Core.CommentAgg.Contracts.Data;
using Servino.Domain.Core.ExpertHomeServiceAgg.Contracts.Data;
using Servino.Domain.Core.HomeServiceAgg.Contracts.Data;
using Servino.Domain.Core.LocationAgg.Contracts.Data;
using Servino.Domain.Core.RequestAgg.Contracts.Data;
using Servino.Domain.Core.SuggestionAgg.Contracts.Data;
using Servino.Domain.Core.UserAgg.Contracts.Data;

namespace Servino.Infa.DataAccess.Repo.EfCore.Repositories;

public static class EfRepositoryServiceRegistration
{
    public static IServiceCollection AddEfRepositoryService(this IServiceCollection services, IConfiguration configuration)
    {
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

        return services;
    }
}