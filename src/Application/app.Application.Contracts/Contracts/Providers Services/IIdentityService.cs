using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.IdentityDTOs;

namespace app.Application.Contracts.Contracts.Providers_Services;

public interface IIdentityService
{
    Task<LoginResultDto> LoginWithPasswordAsync(LoginWithPassDto loginDto, CancellationToken ct);
    Task<IdentityResultDto> RegisterWithEmailAsync(RegisterDto registerDto, CancellationToken ct);
    Task<string> SendOtpAsync(SendOtpDto otpDto, CancellationToken ct);
    Task<LoginResultDto> VerifyOtpAndLoginAsync(LoginWithOtpDto otpDto, CancellationToken ct);
    Task<LoginResultDto> LoginWithGoogleAsync(LoginWithGoogleDto googleDto, CancellationToken ct);
    Task DeleteUserAsync(string id, CancellationToken ct);
    Task<Result<bool>> ChangeEmailAsync(string identityId, string newEmail, CancellationToken ct);
    Task<string?> GetEmailByIdentityIdAsync(string identityId, CancellationToken ct);
    Task<Result<bool>> AdminChangePasswordAsync(string identityId, string newPassword, CancellationToken ct);
    Task LockUserAsync(string identityId, CancellationToken ct);
    Task<Result<bool>> DeactivateUserAsync(string identityId, CancellationToken ct);
    Task<IdentityResultDto> RegisterWithEmailAsync(RegisterDto registerDto, string role, CancellationToken ct);
    Task<Result<bool>> ChangePasswordAsync(string identityId, string currentPassword, string newPassword, CancellationToken ct);
}