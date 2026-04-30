using app.Application.Common;
using app.Application.DTOs.IdentityDTOs;
using MediatR;

namespace app.Application.Features.Users.Commands.VerifyOtpAndLogin;

public record VerifyOtpAndLoginCommand(LoginWithOtpDto Command)
    : IRequest<Result<LoginResultDto>>;
