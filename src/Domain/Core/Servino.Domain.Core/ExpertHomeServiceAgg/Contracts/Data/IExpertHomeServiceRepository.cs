using Servino.Domain.Core.ExpertHomeServiceAgg.Entity;
using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Domain.Core.ExpertHomeServiceAgg.Contracts.Data;

public interface IExpertHomeServiceRepository
{
    Task<List<int>> GetServiceIdsByExpertIdAsync(int expertId, CancellationToken ct);
    Task DeleteAllByExpertIdAsync(int expertId, CancellationToken ct);
    Task AddRangeAsync(List<ExpertHomeService> list, CancellationToken ct);
    Task<List<ExpertServiceItemDto>> GetExpertServicesByExpertIdAsync(int expertId, CancellationToken ct);
}