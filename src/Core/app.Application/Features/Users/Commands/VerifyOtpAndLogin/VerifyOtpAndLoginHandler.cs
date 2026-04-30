using app.Application.Common;
using app.Application.Contracts.Services;
using app.Application.DTOs.IdentityDTOs;
using MediatR;

namespace app.Application.Features.Users.Commands.VerifyOtpAndLogin;

public class VerifyOtpAndLoginHandler(IIdentityService identityService)
    : IRequestHandler<VerifyOtpAndLoginCommand, Result<LoginResultDto>>
{
    public async Task<Result<LoginResultDto>> Handle(VerifyOtpAndLoginCommand request, CancellationToken ct)
    {
        var result = await identityService.VerifyOtpAndLoginAsync(request.Command, ct);
        return result.Succeeded
            ? Result<LoginResultDto>.Success(result)
            : Result<LoginResultDto>.Failure(result.Message ?? "کد وارد شده نامعتبر است.");
    }
}
