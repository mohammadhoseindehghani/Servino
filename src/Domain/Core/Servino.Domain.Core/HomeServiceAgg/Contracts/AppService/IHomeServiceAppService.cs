using Servino.Domain.Core._common;
using Servino.Domain.Core.HomeServiceAgg.Dtos;

namespace Servino.Domain.Core.HomeServiceAgg.Contracts.AppService;

public interface IHomeServiceAppService
{
    Task<Result<bool>> CreateAsync(HomeServiceDto command, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(HomeServiceDto command, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, CancellationToken ct);
    Task<Result<HomeServiceDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<List<HomeServiceSummaryDto>>> GetAllAsync(PaginationRequestDto search, CancellationToken ct);
}