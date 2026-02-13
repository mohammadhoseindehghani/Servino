using Servino.Domain.Core._common;
using Servino.Domain.Core.HomeServiceAgg.Contracts.Data;
using Servino.Domain.Core.HomeServiceAgg.Dtos;
using System.Data;
using Dapper;

namespace Servino.Infa.DataAccess.Repo.Dapper.Repositories;

public class HomeServiceRepositoryDapper(IDbConnection connection) : IHomeServiceDapperRepository
{
    public async Task<List<HomeServiceSummaryDto>> GetAllActiveServicesAsync(CancellationToken ct)
    {
        string sql = @"
SELECT 
    hs.Id,
    hs.Title,
    c.Title AS CategoryName,
    hs.BasePrice,
    hs.VisitCount,
    hs.ImagePath
FROM HomeServices hs
INNER JOIN Categories c ON hs.CategoryId = c.Id
WHERE hs.IsActive = 1 
  AND hs.IsDeleted = 0
ORDER BY hs.VisitCount DESC, hs.Id DESC;";

        var command = new CommandDefinition(
            sql,
            cancellationToken: ct);

        var services = await connection.QueryAsync<HomeServiceSummaryDto>(command);
        return services.AsList();
    }


    public async Task<HomeServiceDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        string sql = @"
SELECT 
    Id,
    Title,
    BasePrice,
    CategoryId,
    ShortDescription,
    ImagePath
FROM HomeServices
WHERE Id = @Id 
  AND IsDeleted = 0;";

        var command = new CommandDefinition(
            sql,
            new { Id = id },
            cancellationToken: ct);

        return await connection.QueryFirstOrDefaultAsync<HomeServiceDto>(command);
    }


    public async Task<List<HomeServiceSummaryDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        var pageNumber = search.PageNumber < 1 ? 1 : search.PageNumber;
        var pageSize = search.PageSize < 1 ? 10 : search.PageSize;
        var offset = (pageNumber - 1) * pageSize;

        var sql = @"
SELECT
    hs.Id,
    hs.Title,
    c.Title AS CategoryName,
    hs.BasePrice,
    hs.VisitCount,
    hs.ImagePath
FROM HomeServices hs
INNER JOIN Categories c ON hs.CategoryId = c.Id
WHERE hs.IsDeleted = 0
  AND (@SearchKey IS NULL OR hs.Title LIKE @SearchKey OR c.Title LIKE @SearchKey)
ORDER BY hs.VisitCount DESC, hs.Id DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        var param = new
        {
            SearchKey = string.IsNullOrWhiteSpace(search.SearchKey) ? null : $"%{search.SearchKey}%",
            Offset = offset,
            PageSize = pageSize
        };

         var result = await connection.QueryAsync<HomeServiceSummaryDto>(
             new CommandDefinition(sql, param, cancellationToken: ct));

        return result.AsList();
    }

}