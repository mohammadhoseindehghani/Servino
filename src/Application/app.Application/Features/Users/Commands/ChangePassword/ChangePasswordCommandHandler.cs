using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services;
using FluentValidation;
using MediatR;

namespace app.Application.Features.Users.Commands.ChangePassword;

public class ChangePasswordCommandHandler(IIdentityService identityService, IUserRepository userRepository,
    IValidator<ChangePasswordCommand> validator)
    : IRequestHandler<ChangePasswordCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var user = await userRepository.GetByIdAsync(request.Command.UserId, ct);
        if (user == null)
            return Result<bool>.Failure("کاربر یافت نشد.");
        var result = await identityService.ChangePasswordAsync(user.IdentityId, request.Command.CurrentPassword, request.Command.NewPassword, ct);

        if (!result.IsSuccess)
            return Result<bool>.Failure("تغییر رمز عبور با خطا مواجه شد.");

        return Result<bool>.Success(true, "رمز عبور با موفقیت تغییر یافت.");
    }
}
