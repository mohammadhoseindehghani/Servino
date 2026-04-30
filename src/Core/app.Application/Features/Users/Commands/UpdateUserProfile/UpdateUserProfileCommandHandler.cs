using app.Application.Common;
using app.Application.Contracts.Repositories;
using MediatR;

namespace app.Application.Features.Users.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandHandler(IUserRepository userRepository)
    : IRequestHandler<UpdateUserProfileCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateUserProfileCommand request, CancellationToken ct)
    {
        var result = await userRepository.UpdateProfileAsync(request.Command, ct);

        if (!result)
            return Result<bool>.Failure("به‌روزرسانی پروفایل با خطا مواجه شد.");

        return Result<bool>.Success(true, "پروفایل با موفقیت به‌روزرسانی شد.");
    }
}
