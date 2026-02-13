using Dapper;
using Servino.Domain.Core._common;
using Servino.Domain.Core.LocationAgg.Contracts.Data;
using Servino.Domain.Core.LocationAgg.Dtos;
using Servino.Infa.DataAccess.Repo.Dapper.Repositories.Queries;
using System.Data;

namespace Servino.Infa.DataAccess.Repo.Dapper.Repositories;

public class ProvinceRepositoryDapper(IDbConnection connection) : IProvinceDapperRepository
{
    public async Task<ProvinceDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var command = new CommandDefinition(
            ProvinceQueries.GetById,
            new { Id = id },
            cancellationToken: ct);

        return await connection.QueryFirstOrDefaultAsync<ProvinceDto>(command);
    }
    public async Task<List<ProvinceDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
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
            ProvinceQueries.GetAllPaged,
            param,
            cancellationToken: ct);

        var provinces = await connection.QueryAsync<ProvinceDto>(command);
        return provinces.AsList();
    }
    public async Task<List<SelectListDto>> GetAllForDropdownAsync(CancellationToken ct)
    {
        var command = new CommandDefinition(
            ProvinceQueries.GetAllForDropdown,
            cancellationToken: ct);

        var provinces = await connection.QueryAsync<SelectListDto>(command);
        return provinces.AsList();
    }
}