using app.Application.Common;
using MediatR;

namespace app.Application.Features.Users.Commands.UpdateProfileImage;

public record UpdateProfileImageCommand(int UserId, string Path)
    : IRequest<Result<bool>>;
