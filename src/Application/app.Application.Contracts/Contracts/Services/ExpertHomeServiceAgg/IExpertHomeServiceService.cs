using app.Application.Contracts.DTOs.HomeServiceDTOs;
using app.Application.Contracts.DTOs.UserDTOs;
using app.Domain.ExpertHomeServiceAgg.Entities;

namespace app.Application.Contracts.Contracts.Services.ExpertHomeServiceAgg;

public interface IExpertHomeServiceService
{
    Task<List<int>> GetSelectedServiceIdsAsync(int expertId, CancellationToken ct);
    Task UpdateExpertServicesAsync(int expertId, List<int> selectedServiceIds, CancellationToken ct);
    Task<List<ExpertServiceItemDto>> GetExpertServicesByExpertIdAsync(int expertId, CancellationToken ct);
    Task DeleteAllByExpertIdAsync(int expertId, CancellationToken ct);
    Task AddRangeAsync(List<ExpertHomeService> list, CancellationToken ct);
    Task<List<int>> GetServiceIdsByExpertIdAsync(int expertId, CancellationToken ct);
}