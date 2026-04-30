using app.Application.Common;
using app.Application.DTOs.IdentityDTOs;
using MediatR;

namespace app.Application.Features.Users.Commands.LoginWithPassword;

public record LoginWithPasswordCommand(LoginWithPassDto Command)
    : IRequest<Result<LoginResultDto>>;
