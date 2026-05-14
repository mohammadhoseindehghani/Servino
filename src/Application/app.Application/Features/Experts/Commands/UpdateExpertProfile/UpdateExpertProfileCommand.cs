using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Experts.Commands.UpdateExpertProfile;

public record UpdateExpertProfileCommand(UpdateExpertProfileDto Profile)
    : IRequest<Result<bool>>;
