using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.IdentityDTOs;
using MediatR;

namespace app.Application.Features.Users.Commands.RegisterUser;

public record RegisterUserCommand(RegisterDto Command)
    : IRequest<Result<bool>>;
