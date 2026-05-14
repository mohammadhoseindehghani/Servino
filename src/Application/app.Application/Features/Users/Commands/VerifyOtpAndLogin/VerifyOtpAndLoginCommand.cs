using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.IdentityDTOs;
using MediatR;

namespace app.Application.Features.Users.Commands.VerifyOtpAndLogin;

public record VerifyOtpAndLoginCommand(LoginWithOtpDto Command)
    : IRequest<Result<LoginResultDto>>;
