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
        string sql = "SELECT Id, Title, ParentId, ImagePath FROM Categories WHERE IsDeleted = 0";

        if (!string.IsNullOrWhiteSpace(search.SearchKey))
        {
            sql += " AND Title LIKE @SearchKey";
        }

        var categories = await connection.QueryAsync<CategorySummaryDto>(sql, new { SearchKey = "%" + search.SearchKey + "%" });

        return categories.AsList();
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