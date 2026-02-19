using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Domain.Core.UserAgg.Contracts.Service;

public interface IExpertService
{
    Task<bool> CreateAsync(int userId, CancellationToken ct);
    Task<ExpertProfileDto?> GetByUserId(int userId, CancellationToken ct);
    Task<bool> UpdateProfile(UpdateExpertProfileDto command, CancellationToken ct);
    Task<int> GetExpertIdByUserIdAsync(int userId, CancellationToken ct);
    Task<ExpertProfileDto?> GetByExpertIdAsync(int userId, CancellationToken ct);
    Task<bool> HardDeleteByUserIdAsync(int userId, CancellationToken ct);
}