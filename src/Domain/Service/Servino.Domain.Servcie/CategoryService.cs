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
}