using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.Contracts.Services;
using MediatR;

namespace app.Application.Features.Users.Commands.ChangePassword;

public class ChangePasswordCommandHandler(IIdentityService identityService, IUserRepository userRepository)
    : IRequestHandler<ChangePasswordCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken ct)
    {
        var user = await userRepository.GetByIdAsync(request.Command.UserId, ct);
        if (user == null)
            return Result<bool>.Failure("کاربر یافت نشد.");
        var result = await identityService.ChangePasswordAsync(user.IdentityId, request.Command.CurrentPassword, request.Command.NewPassword, ct);

        if (!result.IsSuccess)
            return Result<bool>.Failure("تغییر رمز عبور با خطا مواجه شد.");

        return Result<bool>.Success(true, "رمز عبور با موفقیت تغییر یافت.");
    }
}
