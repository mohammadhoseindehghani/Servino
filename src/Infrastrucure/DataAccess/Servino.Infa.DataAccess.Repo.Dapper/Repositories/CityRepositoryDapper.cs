using Servino.Domain.Core._common;
using Servino.Domain.Core.LocationAgg.Contracts.Data;
using Servino.Domain.Core.LocationAgg.Dtos;
using System.Data;
using Dapper;

namespace Servino.Infa.DataAccess.Repo.Dapper.Repositories;

public class CityRepositoryDapper(IDbConnection connection) : ICityDapperRepository
{

    public async Task<CityDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        string sql = @"
SELECT 
    Id, 
    Title, 
    ProvinceId
FROM Cities
WHERE Id = @Id
  AND IsDeleted = 0;";

        var command = new CommandDefinition(
            sql,
            new { Id = id },
            cancellationToken: ct);

        return await connection.QueryFirstOrDefaultAsync<CityDto>(command);
    }


    public async Task<List<CityDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        string sql = "SELECT Id, Title, ProvinceId FROM Cities WHERE IsDeleted = 0";

        if (!string.IsNullOrWhiteSpace(search.SearchKey))
        {
            sql += " AND Title LIKE @SearchKey";
        }

        var cities = await connection.QueryAsync<CityDto>(sql, new { SearchKey = "%" + search.SearchKey + "%" });

        return cities.AsList();
    }

    public async Task<List<SelectListDto>> GetCitiesByProvinceIdAsync(int provinceId, CancellationToken ct)
    {
        string sql = @"
SELECT 
    Id, 
    Title
FROM Cities
WHERE ProvinceId = @ProvinceId
  AND IsDeleted = 0
ORDER BY Title;";

        var command = new CommandDefinition(
            sql,
            new { ProvinceId = provinceId },
            cancellationToken: ct);

        var cities = await connection.QueryAsync<SelectListDto>(command);
        return cities.AsList();
    }

}