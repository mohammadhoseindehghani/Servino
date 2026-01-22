using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Domain.Core.UserAgg.Contracts.Data;

public interface IAdminRepository
{
    Task<AdminProfileDto?> GetByUserIdAsync(int userId, CancellationToken ct);
}