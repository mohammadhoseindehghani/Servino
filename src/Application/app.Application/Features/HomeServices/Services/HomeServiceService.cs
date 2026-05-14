using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.HomeServiceAgg;
using app.Application.Contracts.DTOs.HomeServiceDTOs;

namespace app.Application.Features.HomeServices.Services;

public class HomeServiceService(IHomeServiceRepository homeServiceRepo) : IHomeServiceService
{
    public async Task<List<HomeServiceSummaryDto>> GetAllActiveServicesAsync(CancellationToken ct)
    {
        return await homeServiceRepo.GetAllActiveServicesAsync(ct);
    }

    public async Task<bool> CreateAsync(HomeServiceDto command, CancellationToken ct)
    {
        return await homeServiceRepo.CreateAsync(command, ct);
    }

    public async Task<bool> UpdateAsync(HomeServiceDto command, CancellationToken ct)
    {
        return await homeServiceRepo.UpdateAsync(command, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        return await homeServiceRepo.DeleteAsync(id, ct);
    }

    public async Task<HomeServiceDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await homeServiceRepo.GetByIdAsync(id, ct);
    }

    public async Task<List<HomeServiceSummaryDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        return await homeServiceRepo.GetAllAsync(search, ct);
    }
}