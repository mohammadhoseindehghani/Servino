using Servino.Domain.Core.UserAgg.Dtos.Identity;

namespace Servino.Domain.Core.UserAgg.Contracts.Service;

public interface IIdentityService
{
    Task<LoginResultDto> LoginWithPasswordAsync(LoginWithPassDto loginDto, CancellationToken ct);
    Task<IdentityResultDto> RegisterWithEmailAsync(RegisterDto registerDto, CancellationToken ct);
    Task<string> SendOtpAsync(SendOtpDto otpDto, CancellationToken ct);
    Task<LoginResultDto> VerifyOtpAndLoginAsync(LoginWithOtpDto otpDto, CancellationToken ct);
    Task<LoginResultDto> LoginWithGoogleAsync(LoginWithGoogleDto googleDto, CancellationToken ct);
    Task DeleteUserAsync(string id, CancellationToken ct);
}