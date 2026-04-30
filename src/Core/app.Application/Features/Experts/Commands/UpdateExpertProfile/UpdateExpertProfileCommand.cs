using app.Application.Common;
using app.Application.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Experts.Commands.UpdateExpertProfile;

public record UpdateExpertProfileCommand(UpdateExpertProfileDto Profile)
    : IRequest<Result<bool>>;
