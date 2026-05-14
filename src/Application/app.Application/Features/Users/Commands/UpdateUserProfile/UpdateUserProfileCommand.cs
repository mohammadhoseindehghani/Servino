using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Users.Commands.UpdateUserProfile;

public record UpdateUserProfileCommand(UpdateUserDto Command)
    : IRequest<Result<bool>>;
