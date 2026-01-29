using Servino.Domain.Core._common;
using Servino.Domain.Core.UserAgg.Dtos;
using Servino.Domain.Core.UserAgg.Dtos.Identity;

namespace Servino.Domain.Core.UserAgg.Contracts.AppService;

public interface IUserAppService
{
    Task<Result<bool>> CreateUserByAdminAsync(CreateUserByAdminDto command, CancellationToken ct);
    Task<Result<bool>> UpdateUserProfileAsync(UpdateUserDto command, CancellationToken ct);
    Task<Result<bool>> UpdateEmailAsync(int userId, string newEmail, CancellationToken ct); 
    Task<Result<LoginResultDto>> LoginWithPasswordAsync(LoginWithPassDto command, CancellationToken ct);
    Task<Result<bool>> RegisterUserAsync(RegisterDto command, CancellationToken ct);
    Task<Result<string>> SendOtpAsync(SendOtpDto command, CancellationToken ct);
    Task<Result<LoginResultDto>> VerifyOtpAndLoginAsync(LoginWithOtpDto command, CancellationToken ct);
    Task<Result<LoginResultDto>> LoginWithGoogleAsync(LoginWithGoogleDto command, CancellationToken ct);

    Task<Result<UserDetailDto>> GetUserProfileAsync(int userId, CancellationToken ct);
    Task<Result<bool>> EditUserProfileAsync(UpdateUserDto command, CancellationToken ct);
    Task<Result<bool>> ChangeUserBalanceAsync(int userId, decimal amount, CancellationToken ct);

    Task<Result<List<UserSummaryDto>>> GetUsersListAsync(PaginationRequestDto search, CancellationToken ct);
    Task<Result<bool>> DeleteUserAsync(int userId, CancellationToken ct);
    Task<Result<bool>> AdminUpdateUserAsync(AdminUpdateUserDto command, CancellationToken ct);
    Task<Result<bool>> ChangePasswordAsync(ChangePasswordDto command, CancellationToken ct);
    Task<bool> IsMobileExistAsync(string mobile, CancellationToken ct);

}