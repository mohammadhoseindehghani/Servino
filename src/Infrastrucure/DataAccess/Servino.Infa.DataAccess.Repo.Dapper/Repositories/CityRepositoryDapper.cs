using Dapper;
using Servino.Domain.Core._common;
using Servino.Domain.Core.LocationAgg.Contracts.Data;
using Servino.Domain.Core.LocationAgg.Dtos;
using Servino.Infa.DataAccess.Repo.Dapper.Repositories.Queries;
using System.Data;

namespace Servino.Infa.DataAccess.Repo.Dapper.Repositories;

public class CityRepositoryDapper(IDbConnection connection) : ICityDapperRepository
{
    public async Task<CityDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var command = new CommandDefinition(
            CityQueries.GetById,
            new { Id = id },
            cancellationToken: ct);

        return await connection.QueryFirstOrDefaultAsync<CityDto>(command);
    }
    public async Task<List<CityDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        var pageNumber = search.PageNumber < 1 ? 1 : search.PageNumber;
        var pageSize = search.PageSize < 1 ? 10 : search.PageSize;
        var offset = (pageNumber - 1) * pageSize;

        var param = new
        {
            SearchKey = string.IsNullOrWhiteSpace(search.SearchKey) ? null : $"%{search.SearchKey}%",
            Offset = offset,
            PageSize = pageSize
        };

        var command = new CommandDefinition(
            CityQueries.GetAllPaged,
            param,
            cancellationToken: ct);

        var cities = await connection.QueryAsync<CityDto>(command);
        return cities.AsList();
    }
    public async Task<List<SelectListDto>> GetCitiesByProvinceIdAsync(int provinceId, CancellationToken ct)
    {
        var command = new CommandDefinition(
            CityQueries.GetByProvinceIdForDropdown,
            new { ProvinceId = provinceId },
            cancellationToken: ct);

        var cities = await connection.QueryAsync<SelectListDto>(command);
        return cities.AsList();
    }
}