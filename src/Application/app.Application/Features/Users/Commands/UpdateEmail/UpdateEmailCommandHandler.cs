using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Providers_Services;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.DTOs.UserDTOs;
using FluentValidation;
using MediatR;

namespace app.Application.Features.Users.Commands.UpdateEmail;

public class UpdateEmailCommandHandler(
    IUserRepository userRepository,
    IIdentityService identityService,
    IValidator<UpdateEmailCommand> validator)
    : IRequestHandler<UpdateEmailCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(UpdateEmailCommand request, CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var user = await userRepository.GetByIdAsync(request.UserId, ct);
        if (user == null) return Result<bool>.Failure("کاربر یافت نشد.");

        if (await userRepository.IsEmailExistAsync(request.NewEmail, ct))
        {
            var currentEmail = await identityService.GetEmailByIdentityIdAsync(user.IdentityId, ct);
            if (currentEmail != request.NewEmail)
                return Result<bool>.Failure("این ایمیل قبلاً استفاده شده است.");
        }

        var result = await identityService.ChangeEmailAsync(user.IdentityId, request.NewEmail, ct);
        if (!result.IsSuccess) return result;

        var updateUserDto = new UpdateUserDto
        {
            Id = request.UserId,
            Email = request.NewEmail
        };
        await userRepository.UpdateAsync(updateUserDto, ct);
        return Result<bool>.Success(true, "ایمیل با موفقیت تغییر یافت.");
    }
}


