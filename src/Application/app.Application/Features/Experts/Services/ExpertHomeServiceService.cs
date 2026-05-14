using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.ExpertHomeServiceAgg;
using app.Application.Contracts.DTOs.HomeServiceDTOs;
using app.Application.Contracts.DTOs.UserDTOs;
using app.Domain.ExpertHomeServiceAgg.Entities;

namespace app.Application.Features.Experts.Services;

public class ExpertHomeServiceService(IExpertHomeServiceRepository repo) : IExpertHomeServiceService
{
    public async Task<List<int>> GetSelectedServiceIdsAsync(int expertId, CancellationToken ct)
    {
        return await repo.GetServiceIdsByExpertIdAsync(expertId, ct);
    }

    public async Task UpdateExpertServicesAsync(int expertId, List<int> selectedServiceIds, CancellationToken ct)
    {
        await repo.DeleteAllByExpertIdAsync(expertId, ct);

        if (selectedServiceIds != null! && selectedServiceIds.Any())
        {
            var list = selectedServiceIds.Select(id => new ExpertHomeService
            {
                ExpertId = expertId,
                HomeServiceId = id
            }).ToList();

            await repo.AddRangeAsync(list, ct);
        }
    }

    public async Task<List<ExpertServiceItemDto>> GetExpertServicesByExpertIdAsync(int expertId, CancellationToken ct)
    {
        return await repo.GetExpertServicesByExpertIdAsync(expertId, ct);
    }

    public async Task DeleteAllByExpertIdAsync(int expertId, CancellationToken ct)
    { 
        await repo.DeleteAllByExpertIdAsync(expertId, ct);
    }

    public async Task AddRangeAsync(List<ExpertHomeService> list, CancellationToken ct)
    {
        await repo.AddRangeAsync(list, ct);
    }

    public async Task<List<int>> GetServiceIdsByExpertIdAsync(int expertId, CancellationToken ct)
    {
        return await repo.GetServiceIdsByExpertIdAsync(expertId, ct);
    }
}