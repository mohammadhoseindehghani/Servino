using Servino.Domain.Core._common;
using Servino.Domain.Core.HomeServiceAgg.Dtos;

namespace Servino.Domain.Core.HomeServiceAgg.Contracts.Data;

public interface IHomeServiceDapperRepository
{
    Task<List<HomeServiceSummaryDto>> GetAllActiveServicesAsync(CancellationToken ct);
    Task<HomeServiceDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<List<HomeServiceSummaryDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct);


}