using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Users.Commands.ChangePassword;

public record ChangePasswordCommand(ChangePasswordDto Command)
    : IRequest<Result<bool>>;
