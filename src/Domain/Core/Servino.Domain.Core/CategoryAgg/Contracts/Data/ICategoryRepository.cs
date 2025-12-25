using Servino.Domain.Core._common;
using Servino.Domain.Core.CategoryAgg.Dtos;

namespace Servino.Domain.Core.CategoryAgg.Contracts.Data;

public interface ICategoryRepository
{
    Task<bool> CreateAsync(CategoryDto command, CancellationToken ct);
    Task<bool> UpdateAsync(CategoryDto command, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct); 
    Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<List<CategorySummaryDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct);
    Task<int> GetCountAsync(CancellationToken ct);
}
