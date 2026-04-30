using app.Application.Common;
using app.Application.DTOs.IdentityDTOs;
using MediatR;

namespace app.Application.Features.Users.Commands.RegisterUser;

public record RegisterUserCommand(RegisterDto Command)
    : IRequest<Result<bool>>;
