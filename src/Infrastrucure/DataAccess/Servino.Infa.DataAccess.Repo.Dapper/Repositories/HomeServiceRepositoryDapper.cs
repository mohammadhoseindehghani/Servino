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
        string sql = "SELECT hs.Id, hs.Title, hs.BasePrice, hs.VisitCount, hs.ImagePath, c.Title AS CategoryName " +
                     "FROM HomeServices hs " +
                     "INNER JOIN Categories c ON hs.CategoryId = c.Id " +
                     "WHERE hs.IsActive = 1 AND hs.IsDeleted = 0";

        var services = await connection.QueryAsync<HomeServiceSummaryDto>(sql);
        return services.AsList();
    }

    public async Task<HomeServiceDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        string sql = "SELECT Id, Title, BasePrice, CategoryId, ShortDescription, ImagePath FROM HomeServices WHERE Id = @Id AND IsDeleted = 0";
        return await connection.QueryFirstOrDefaultAsync<HomeServiceDto>(sql, new { Id = id });
    }

    public async Task<List<HomeServiceSummaryDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        string sql = "SELECT hs.Id, hs.Title, hs.BasePrice, hs.VisitCount, hs.ImagePath, c.Title AS CategoryName " +
                     "FROM HomeServices hs " +
                     "INNER JOIN Categories c ON hs.CategoryId = c.Id " +
                     "WHERE hs.IsDeleted = 0";

        if (!string.IsNullOrWhiteSpace(search.SearchKey))
        {
            sql += " AND hs.Title LIKE @SearchKey";
        }

        var services = await connection.QueryAsync<HomeServiceSummaryDto>(sql, new { SearchKey = "%" + search.SearchKey + "%" });
        return services.AsList();
    }
}