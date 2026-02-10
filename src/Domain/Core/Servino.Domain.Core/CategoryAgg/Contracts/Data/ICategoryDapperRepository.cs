using Servino.Domain.Core._common;
using Servino.Domain.Core.CategoryAgg.Dtos;

namespace Servino.Domain.Core.CategoryAgg.Contracts.Data;

public interface ICategoryDapperRepository
{
    Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<List<CategorySummaryDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct);
    Task<List<CategoryClientDto>> GetCategoriesByParentIdAsync(int? parentId, CancellationToken ct);
    Task<List<ServiceClientDto>> GetServicesByCategoryIdAsync(int categoryId, CancellationToken ct);

}