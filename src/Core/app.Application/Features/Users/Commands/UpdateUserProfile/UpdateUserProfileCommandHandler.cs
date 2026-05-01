using app.Application.Common;
using app.Application.Contracts.Repositories;
using FluentValidation;
using MediatR;

namespace app.Application.Features.Users.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandHandler(IUserRepository userRepository, IValidator<UpdateUserProfileCommand> validator)
    : IRequestHandler<UpdateUserProfileCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateUserProfileCommand request, CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var result = await userRepository.UpdateProfileAsync(request.Command, ct);

        if (!result)
            return Result<bool>.Failure("به‌روزرسانی پروفایل با خطا مواجه شد.");

        return Result<bool>.Success(true, "پروفایل با موفقیت به‌روزرسانی شد.");
    }
}
