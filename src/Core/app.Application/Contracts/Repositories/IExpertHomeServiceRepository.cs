using app.Application.DTOs.UserDTOs;
using app.Domain.ExpertHomeServiceAgg.Entities;

namespace app.Application.Contracts.Repositories;

public interface IExpertHomeServiceRepository
{
    Task<List<int>> GetServiceIdsByExpertIdAsync(int expertId, CancellationToken ct);
    Task DeleteAllByExpertIdAsync(int expertId, CancellationToken ct);
    Task AddRangeAsync(List<ExpertHomeService> list, CancellationToken ct);
    Task<List<ExpertServiceItemDto>> GetExpertServicesByExpertIdAsync(int expertId, CancellationToken ct);
}