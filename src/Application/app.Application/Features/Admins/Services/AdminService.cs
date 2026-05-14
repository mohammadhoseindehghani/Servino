using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.UserAgg;
using app.Application.Contracts.DTOs.UserDTOs;

namespace app.Application.Features.Admins.Services;

public class AdminService(IAdminRepository adminRepo) : IAdminService
{
    public async Task<AdminProfileDto?> GetByUserIdAsync(int userId, CancellationToken ct)
    {
        return await adminRepo.GetByUserIdAsync(userId, ct);
    }
}