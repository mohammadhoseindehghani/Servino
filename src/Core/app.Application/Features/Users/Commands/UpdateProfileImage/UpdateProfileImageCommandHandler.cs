using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.Contracts.Services;
using app.Domain.UserAgg.Entities;
using FluentValidation;
using MediatR;

namespace app.Application.Features.Users.Commands.UpdateProfileImage;

public class UpdateProfileImageCommandHandler(
    IUserRepository userRepository,
    IFileService fileService,
    IValidator<UpdateProfileImageCommand> validator)
    : IRequestHandler<UpdateProfileImageCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(UpdateProfileImageCommand request, CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var result = await userRepository.UpdateProfileImageAsync(request.UserId, request.Path, ct);
        return !result ? Result<bool>.Failure("عملیات به روز رسانی عکس پروفایل با شکست مواجه شد.")
            : Result<bool>.Success(result);
    }
}
