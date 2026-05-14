using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.CategoryDTOs;
using app.Domain.CategoryAgg.Entities;

namespace app.Application.Contracts.Contracts.Services.CategoryAgg;

public interface ICategoryService
{
    Task<int> CreateAsync(Category command, CancellationToken ct);
    Task UpdateAsync(CategoryDto command, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
    Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<List<CategorySummaryDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct);
    Task<int> GetCountAsync(CancellationToken ct);

    Task<List<CategoryClientDto>> GetCategoriesByParentIdAsync(int? parentId, CancellationToken ct);
    Task<List<ServiceClientDto>> GetServicesByCategoryIdAsync(int categoryId, CancellationToken ct);
    Task<List<BreadcrumbDto>> GetBreadcrumbAsync(int categoryId, CancellationToken ct);
    Task<bool> IsCategoryExistAndActiveAsync(int id, CancellationToken ct);
}