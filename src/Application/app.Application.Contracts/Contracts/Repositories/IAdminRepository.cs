using app.Application.Contracts.DTOs.UserDTOs;

namespace app.Application.Contracts.Contracts.Repositories;

public interface IAdminRepository
{
    Task<AdminProfileDto?> GetByUserIdAsync(int userId, CancellationToken ct);
}