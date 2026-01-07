using Servino.Domain.Core._common;
using Servino.Domain.Core.UserAgg.Contracts.Data;
using Servino.Domain.Core.UserAgg.Contracts.Service;
using Servino.Domain.Core.UserAgg.Dtos;
using Servino.Domain.Core.UserAgg.Dtos.Identity;

namespace Servino.Domain.Service;

public class UserService(IUserRepository userRepo) : IUserService
{
    public async Task<UserProfileDto?> GetProfileByIdAsync(int userId, string role, CancellationToken ct)
    {
        return await userRepo.GetProfileByIdAsync(userId, role, ct);
    }

    public async Task<bool> UpdateProfileAsync(UpdateUserDto command, CancellationToken ct)
    {
        return await userRepo.UpdateProfileAsync(command, ct);
    }

    public async Task<bool> CreateAsync(CreateUserDto command, CancellationToken ct)
    {
        return await userRepo.CreateAsync(command, ct);
    }

    public async Task<bool> UpdateAsync(UpdateUserDto command, CancellationToken ct)
    {
        return await userRepo.UpdateAsync(command, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        return await userRepo.DeleteAsync(id, ct);
    }

    public async Task<UserDetailDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await userRepo.GetByIdAsync(id, ct);
    }

    public async Task<int> GetIdByIdentityIdAsync(string identityId, CancellationToken ct)
    {
        return await userRepo.GetIdByIdentityIdAsync(identityId , ct);
    }

    public async Task<List<UserSummaryDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        return await userRepo.GetAllAsync(search, ct);
    }

    public async Task<bool> ChangeBalanceAsync(int userId, decimal amount, CancellationToken ct)
    {
        return await userRepo.ChangeBalanceAsync(userId, amount, ct);
    }

    public async Task<bool> IsEmailExistAsync(string email, CancellationToken ct)
    {
        return await userRepo.IsEmailExistAsync(email, ct);
    }

    public async Task<int> GetCountAsync(CancellationToken ct)
    {
        return await userRepo.GetCountAsync(ct);
    }
}