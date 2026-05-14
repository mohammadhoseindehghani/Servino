using app.Application.Contracts.DTOs.UserDTOs;

namespace app.Application.Contracts.Contracts.Services.ExpertHomeServiceAgg;

public interface IExpertHomeServiceService
{
    Task<List<int>> GetSelectedServiceIdsAsync(int expertId, CancellationToken ct);
    Task UpdateExpertServicesAsync(int expertId, List<int> selectedServiceIds, CancellationToken ct);
    Task<List<ExpertServiceItemDto>> GetExpertServicesByExpertIdAsync(int expertId, CancellationToken ct);
}