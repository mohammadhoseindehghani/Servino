using app.Application.Contracts.DTOs.UserDTOs;

namespace app.Application.Contracts.Contracts.Services.UserAgg;

public interface IAdminService
{
    Task<AdminProfileDto?> GetByUserIdAsync(int userId, CancellationToken ct);
}