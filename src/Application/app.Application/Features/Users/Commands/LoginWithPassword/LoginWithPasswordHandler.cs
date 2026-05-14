using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Services;
using app.Application.Contracts.DTOs.IdentityDTOs;
using MediatR;

namespace app.Application.Features.Users.Commands.LoginWithPassword;

public class LoginWithPasswordHandler(IIdentityService identityService)
    : IRequestHandler<LoginWithPasswordCommand, Result<LoginResultDto>>
{
    public async Task<Result<LoginResultDto>> Handle(LoginWithPasswordCommand request, CancellationToken ct)
    {
        var result = await identityService.LoginWithPasswordAsync(request.Command, ct);
        return result.Succeeded
            ? Result<LoginResultDto>.Success(result)
            : Result<LoginResultDto>.Failure(result.Message ?? "نام کاربری یا رمز عبور اشتباه است.");
    }
}
