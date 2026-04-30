using app.Application.Common;
using app.Application.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Users.Commands.AdminUpdateUser;

public record AdminUpdateUserCommand(AdminUpdateUserDto Command)
    : IRequest<Result<bool>>;
