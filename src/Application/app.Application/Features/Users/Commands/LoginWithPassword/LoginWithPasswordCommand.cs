using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.IdentityDTOs;
using MediatR;

namespace app.Application.Features.Users.Commands.LoginWithPassword;

public record LoginWithPasswordCommand(LoginWithPassDto Command)
    : IRequest<Result<LoginResultDto>>;
