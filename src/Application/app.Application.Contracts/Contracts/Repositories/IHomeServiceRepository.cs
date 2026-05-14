using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.HomeServiceDTOs;

namespace app.Application.Contracts.Contracts.Repositories;

public interface IHomeServiceRepository
{
    Task<List<HomeServiceSummaryDto>> GetAllActiveServicesAsync(CancellationToken ct);
    Task<bool> CreateAsync(HomeServiceDto command, CancellationToken ct);
    Task<bool> UpdateAsync(HomeServiceDto command, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<HomeServiceDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<List<HomeServiceSummaryDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct);
}