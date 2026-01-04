using Servino.Domain.Core.ExpertHomeServiceAgg.Contracts.Data;
using Servino.Domain.Core.ExpertHomeServiceAgg.Contracts.Service;
using Servino.Domain.Core.ExpertHomeServiceAgg.Entity;

namespace Servino.Domain.Service;

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
}