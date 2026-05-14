using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.UserAgg;
using FluentValidation;
using MediatR;

namespace app.Application.Features.Users.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandHandler(IUserService userService, IValidator<UpdateUserProfileCommand> validator)
    : IRequestHandler<UpdateUserProfileCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateUserProfileCommand request, CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var result = await userService.UpdateProfileAsync(request.Command, ct);

        if (!result)
            return Result<bool>.Failure("به‌روزرسانی پروفایل با خطا مواجه شد.");

        return Result<bool>.Success(true, "پروفایل با موفقیت به‌روزرسانی شد.");
    }
}
