using Servino.Domain.Core.UserAgg.Contracts.Data;
using Servino.Domain.Core.UserAgg.Contracts.Service;
using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Domain.Service;

public class AdminService(IAdminRepository adminRepo) : IAdminService
{
    public async Task<AdminProfileDto?> GetByUserIdAsync(int userId, CancellationToken ct)
    {
        return await adminRepo.GetByUserIdAsync(userId, ct);
    }
}