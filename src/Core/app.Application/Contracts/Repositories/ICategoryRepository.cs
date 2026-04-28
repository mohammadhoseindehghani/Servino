using app.Application.Common;
using app.Application.DTOs.CategoryDTOs;

namespace app.Application.Contracts.Repositories;

public interface ICategoryRepository
{
    Task<bool> CreateAsync(CategoryDto command, CancellationToken ct);
    Task<bool> UpdateAsync(CategoryDto command, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct); 
    Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<List<CategorySummaryDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct);
    Task<int> GetCountAsync(CancellationToken ct);

    Task<List<CategoryClientDto>> GetCategoriesByParentIdAsync(int? parentId, CancellationToken ct);
    Task<List<ServiceClientDto>> GetServicesByCategoryIdAsync(int categoryId, CancellationToken ct);
    Task<List<BreadcrumbDto>> GetBreadcrumbAsync(int categoryId, CancellationToken ct);
    Task<bool> IsCategoryExistAndActiveAsync(int id, CancellationToken ct);
}
