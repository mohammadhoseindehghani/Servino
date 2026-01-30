using Servino.Domain.Core._common;
using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Domain.Core.UserAgg.Contracts.Data;

public interface IUserRepository
{
    Task<bool> IsMobileExistAsync(string mobile, CancellationToken ct);
    Task<bool> UpdateProfileAsync(UpdateUserDto command, CancellationToken ct);

    Task<bool> CreateAsync(CreateUserDto command, CancellationToken ct);
    Task<bool> UpdateAsync(UpdateUserDto command, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct); 
    Task<bool> HardDeleteAsync(int id, CancellationToken ct); 
    Task<UserDetailDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<int> GetIdByIdentityIdAsync(string identityId, CancellationToken ct);
    Task<List<UserSummaryProjectionDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct);
    Task<bool> ChangeBalanceAsync(int userId, decimal amount, CancellationToken ct);
    Task<bool> IsEmailExistAsync(string email, CancellationToken ct);
    Task<int> GetCountAsync(CancellationToken ct);

    Task<string> GetUserProfileImageAsync(int userId, CancellationToken ct);
    Task<bool> UpdateProfileImageAsync(int userId, string path, CancellationToken ct);
}