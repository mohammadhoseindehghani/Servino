using app.Application.Common;
using app.Application.DTOs.IdentityDTOs;
using MediatR;

namespace app.Application.Features.Users.Commands.SendOtpCommand;

public record SendOtpCommand(SendOtpDto Command)
    : IRequest<Result<string>>;
