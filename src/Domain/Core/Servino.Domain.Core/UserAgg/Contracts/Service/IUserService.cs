using Servino.Domain.Core._common;
using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Domain.Core.UserAgg.Contracts.Service;

public interface IUserService
{
    Task<bool> CreateAsync(CreateUserDto command, CancellationToken ct);
    Task<bool> UpdateAsync(UpdateUserDto command, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
    Task<UserDetailDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<List<UserSummaryDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct);
    Task<bool> ChangeBalanceAsync(int userId, decimal amount, CancellationToken ct);
    Task<bool> IsEmailExistAsync(string email, CancellationToken ct);
    Task<int> GetCountAsync(CancellationToken ct);
}