using Dapper;
using Servino.Domain.Core._common;
using Servino.Domain.Core.HomeServiceAgg.Contracts.Data;
using Servino.Domain.Core.HomeServiceAgg.Dtos;
using Servino.Infa.DataAccess.Repo.Dapper.Repositories.Queries;
using System.Data;

namespace Servino.Infa.DataAccess.Repo.Dapper.Repositories;

public class HomeServiceRepositoryDapper(IDbConnection connection) : IHomeServiceDapperRepository
{
    public async Task<List<HomeServiceSummaryDto>> GetAllActiveServicesAsync(CancellationToken ct)
    {
        var command = new CommandDefinition(
            HomeServiceQueries.GetAllActive,
            cancellationToken: ct);

        var services = await connection.QueryAsync<HomeServiceSummaryDto>(command);
        return services.AsList();
    }

    public async Task<HomeServiceDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var command = new CommandDefinition(
            HomeServiceQueries.GetById,
            new { Id = id },
            cancellationToken: ct);

        return await connection.QueryFirstOrDefaultAsync<HomeServiceDto>(command);
    }

    public async Task<List<HomeServiceSummaryDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
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
            HomeServiceQueries.GetAllPaged,
            param,
            cancellationToken: ct);

        var result = await connection.QueryAsync<HomeServiceSummaryDto>(command);
        return result.AsList();
    }
}