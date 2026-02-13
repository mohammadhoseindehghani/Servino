using Servino.Domain.Core._common;
using Servino.Domain.Core.CategoryAgg.Contracts.Data;
using Servino.Domain.Core.CategoryAgg.Dtos;
using System.Data;
using Dapper;

namespace Servino.Infa.DataAccess.Repo.Dapper.Repositories;

public class CategoryRepositoryDapper(IDbConnection connection) : ICategoryDapperRepository
{

    public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        string sql = "SELECT Id, Title, ParentId, ImagePath FROM Categories WHERE Id = @Id AND IsDeleted = 0";
        return await connection.QueryFirstOrDefaultAsync<CategoryDto>(sql, new { Id = id });
    }

    public async Task<List<CategorySummaryDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        var pageNumber = search.PageNumber < 1 ? 1 : search.PageNumber;
        var pageSize = search.PageSize < 1 ? 10 : search.PageSize;
        var offset = (pageNumber - 1) * pageSize;

        var sql = @"
SELECT
    c.Id,
    c.Title,
    p.Title AS ParentTitle,
    (
        SELECT COUNT(1)
        FROM Categories sc
        WHERE sc.ParentId = c.Id
          AND sc.IsDeleted = 0
    ) AS SubCategoriesCount,
    c.IsActive,
    c.ImagePath
FROM Categories c
LEFT JOIN Categories p ON p.Id = c.ParentId
WHERE c.IsDeleted = 0
  AND (@SearchKey IS NULL OR c.Title LIKE @SearchKey OR p.Title LIKE @SearchKey)
ORDER BY c.CreatedAt DESC, c.Id DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        var param = new
        {
            SearchKey = string.IsNullOrWhiteSpace(search.SearchKey) ? null : $"%{search.SearchKey}%",
            Offset = offset,
            PageSize = pageSize
        };

        var result = await connection.QueryAsync<CategorySummaryDto>(
            new CommandDefinition(sql, param, cancellationToken: ct));

        return result.AsList();
    }



    public async Task<List<CategoryClientDto>> GetCategoriesByParentIdAsync(int? parentId, CancellationToken ct)
    {
        string sql = "SELECT Id, Title, ImagePath FROM Categories WHERE ParentId = @ParentId AND IsDeleted = 0 AND IsActive = 1";
        var categories = await connection.QueryAsync<CategoryClientDto>(sql, new { ParentId = parentId });
        return categories.AsList();
    }

    public async Task<List<ServiceClientDto>> GetServicesByCategoryIdAsync(int categoryId, CancellationToken ct)
    {
        string sql = "SELECT Id, Title, BasePrice, ShortDescription, ImagePath FROM HomeServices WHERE CategoryId = @CategoryId AND IsDeleted = 0";
        var services = await connection.QueryAsync<ServiceClientDto>(sql, new { CategoryId = categoryId });
        return services.AsList();
    }

}