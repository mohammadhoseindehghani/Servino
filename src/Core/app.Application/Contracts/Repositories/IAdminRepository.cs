using app.Application.DTOs.UserDTOs;

namespace app.Application.Contracts.Repositories;

public interface IAdminRepository
{
    Task<AdminProfileDto?> GetByUserIdAsync(int userId, CancellationToken ct);
}