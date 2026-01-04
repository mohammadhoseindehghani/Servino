namespace Servino.Domain.Core.ExpertHomeServiceAgg.Contracts.Service;

public interface IExpertHomeServiceService
{
    Task<List<int>> GetSelectedServiceIdsAsync(int expertId, CancellationToken ct);
    Task UpdateExpertServicesAsync(int expertId, List<int> selectedServiceIds, CancellationToken ct);
}