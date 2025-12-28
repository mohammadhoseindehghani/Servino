using Servino.Domain.Core._common;
using Servino.Domain.Core.HomeServiceAgg.Contracts.Data;
using Servino.Domain.Core.HomeServiceAgg.Contracts.Service;
using Servino.Domain.Core.HomeServiceAgg.Dtos;

namespace Servino.Domain.Service;

public class HomeServiceService(IHomeServiceRepository homeServiceRepo) : IHomeServiceService
{
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