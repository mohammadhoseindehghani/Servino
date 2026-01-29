using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Domain.Core.UserAgg.Contracts.Data;

public interface IExpertRepository
{
    Task<bool> CreateAsync(int userId, CancellationToken ct); 
    Task<int> GetIdByUserIdAsync(int userId, CancellationToken ct);
    Task<ExpertProfileDto?> GetByUserIdAsync(int userId, CancellationToken ct);
    Task<bool> UpdateProfile(UpdateExpertProfileDto command, CancellationToken ct);
    Task<bool> HardDeleteByUserIdAsync(int userId, CancellationToken ct);
}