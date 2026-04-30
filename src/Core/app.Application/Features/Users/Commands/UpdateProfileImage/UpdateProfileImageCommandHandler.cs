using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.Contracts.Services;
using app.Domain.UserAgg.Entities;
using MediatR;

namespace app.Application.Features.Users.Commands.UpdateProfileImage;

public class UpdateProfileImageCommandHandler(
    IUserRepository userRepository,
    IFileService fileService)
    : IRequestHandler<UpdateProfileImageCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(UpdateProfileImageCommand request, CancellationToken ct)
    {
        var result = await userRepository.UpdateProfileImageAsync(request.UserId, request.Path, ct);
        return !result ? Result<bool>.Failure("عملیات به روز رسانی عکس پروفایل با شکست مواجه شد.")
            : Result<bool>.Success(result);
    }
}
