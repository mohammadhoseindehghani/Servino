using Dapper;
using Servino.Domain.Core._common;
using Servino.Domain.Core.CategoryAgg.Contracts.Data;
using Servino.Domain.Core.CategoryAgg.Dtos;
using Servino.Infa.DataAccess.Repo.Dapper.Repositories.Queries;
using System.Data;

namespace Servino.Infa.DataAccess.Repo.Dapper.Repositories;

public class CategoryRepositoryDapper(IDbConnection connection) : ICategoryDapperRepository
{
    public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var command = new CommandDefinition(
            CategoryQueries.GetById,
            new { Id = id },
            cancellationToken: ct);

        return await connection.QueryFirstOrDefaultAsync<CategoryDto>(command);
    }

    public async Task<List<CategorySummaryDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
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
            CategoryQueries.GetAllPaged,
            param,
            cancellationToken: ct);

        var result = await connection.QueryAsync<CategorySummaryDto>(command);
        return result.AsList();
    }

    public async Task<List<CategoryClientDto>> GetCategoriesByParentIdAsync(int? parentId, CancellationToken ct)
    {
        var command = new CommandDefinition(
            CategoryQueries.GetByParentId,
            new { ParentId = parentId },
            cancellationToken: ct);

        var categories = await connection.QueryAsync<CategoryClientDto>(command);
        return categories.AsList();
    }

    public async Task<List<ServiceClientDto>> GetServicesByCategoryIdAsync(int categoryId, CancellationToken ct)
    {
        var command = new CommandDefinition(
            CategoryQueries.GetServicesByCategoryId,
            new { CategoryId = categoryId },
            cancellationToken: ct);

        var services = await connection.QueryAsync<ServiceClientDto>(command);
        return services.AsList();
    }
}