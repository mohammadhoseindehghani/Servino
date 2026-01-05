using Servino.Domain.Core._common;
using Servino.Domain.Core.CategoryAgg.Contracts.Data;
using Servino.Domain.Core.CategoryAgg.Contracts.Service;
using Servino.Domain.Core.CategoryAgg.Dtos;

namespace Servino.Domain.Service;

public class CategoryService(ICategoryRepository categoryRepo) : ICategoryService
{
    public async Task<bool> CreateAsync(CategoryDto command, CancellationToken ct)
    {
        return await categoryRepo.CreateAsync(command, ct);
    }

    public async Task<bool> UpdateAsync(CategoryDto command, CancellationToken ct)
    {
        return await categoryRepo.UpdateAsync(command, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        return await categoryRepo.DeleteAsync(id, ct);
    }

    public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await categoryRepo.GetByIdAsync(id, ct);
    }

    public async Task<List<CategorySummaryDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        return await categoryRepo.GetAllAsync(search, ct);
    }

    public async Task<int> GetCountAsync(CancellationToken ct)
    {
        return await categoryRepo.GetCountAsync(ct);
    }

    public async Task<List<CategoryClientDto>> GetCategoriesByParentIdAsync(int? parentId, CancellationToken ct)
    {
        return await categoryRepo.GetCategoriesByParentIdAsync(parentId, ct);
    }

    public async Task<List<ServiceClientDto>> GetServicesByCategoryIdAsync(int categoryId, CancellationToken ct)
    {
        return await categoryRepo.GetServicesByCategoryIdAsync(categoryId, ct);
    }

    public async Task<List<BreadcrumbDto>> GetBreadcrumbAsync(int categoryId, CancellationToken ct)
    {
        return await categoryRepo.GetBreadcrumbAsync(categoryId, ct);
    }

    public async Task<bool> IsCategoryExistAndActiveAsync(int id, CancellationToken ct)
    {
        return await categoryRepo.IsCategoryExistAndActiveAsync(id, ct);
    }
}