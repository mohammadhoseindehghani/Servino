using Servino.Domain.Core.UserAgg.Dtos;
using Servino.Domain.Core.UserAgg.Entity;

namespace Servino.Domain.Core.UserAgg.Contracts.Data;

public interface IExpertRepository
{
    Task<int> SaveChangesAsync(CancellationToken ct);
    Task<int> GetIdByUserIdAsync(int userId, CancellationToken ct);
    Task<ExpertProfileDto?> GetByUserIdAsync(int userId, CancellationToken ct);
    Task<bool> UpdateProfile(UpdateExpertProfileDto command, CancellationToken ct);
}