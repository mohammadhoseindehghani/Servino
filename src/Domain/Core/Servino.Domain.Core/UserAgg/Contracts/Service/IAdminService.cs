using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Domain.Core.UserAgg.Contracts.Service;

public interface IAdminService
{
    Task<AdminProfileDto?> GetByUserIdAsync(int userId, CancellationToken ct);
}