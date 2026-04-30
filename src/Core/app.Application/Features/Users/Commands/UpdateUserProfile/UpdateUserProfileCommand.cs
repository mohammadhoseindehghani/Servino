using app.Application.Common;
using app.Application.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Users.Commands.UpdateUserProfile;

public record UpdateUserProfileCommand(UpdateUserDto Command)
    : IRequest<Result<bool>>;
