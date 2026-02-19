using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Servino.Domain.Core.CategoryAgg.Contracts.Data;
using Servino.Domain.Core.HomeServiceAgg.Contracts.Data;
using Servino.Domain.Core.LocationAgg.Contracts.Data;

namespace Servino.Infa.DataAccess.Repo.Dapper.Repositories;

public static class DapperRepositoryServiceRegistration
{
    public static IServiceCollection AddDapperRepositoryServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICategoryDapperRepository, CategoryRepositoryDapper>();
        services.AddScoped<IHomeServiceDapperRepository, HomeServiceRepositoryDapper>();
        services.AddScoped<ICityDapperRepository, CityRepositoryDapper>();
        services.AddScoped<IProvinceDapperRepository, ProvinceRepositoryDapper>();

        return services;
    }
}