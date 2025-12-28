using Servino.Domain.Core._common;
using Servino.Domain.Core.CategoryAgg.Dtos;

namespace Servino.Domain.Core.CategoryAgg.Contracts.AppService;

public interface ICategoryAppService
{
    Task<Result<bool>> CreateAsync(CategoryDto command, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(CategoryDto command, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, CancellationToken ct);
    Task<Result<CategoryDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<List<CategorySummaryDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct);
    Task<int> GetCountAsync(CancellationToken ct);
}