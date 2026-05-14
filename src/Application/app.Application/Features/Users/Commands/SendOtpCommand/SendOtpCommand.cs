using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.IdentityDTOs;
using MediatR;

namespace app.Application.Features.Users.Commands.SendOtpCommand;

public record SendOtpCommand(SendOtpDto Command)
    : IRequest<Result<string>>;
