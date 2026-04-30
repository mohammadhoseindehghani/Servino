using app.Application.Common;
using app.Application.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Users.Commands.ChangePassword;

public record ChangePasswordCommand(ChangePasswordDto Command)
    : IRequest<Result<bool>>;
