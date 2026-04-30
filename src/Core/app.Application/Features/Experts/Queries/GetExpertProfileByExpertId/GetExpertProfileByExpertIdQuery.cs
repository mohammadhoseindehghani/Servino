using app.Application.Common;
using app.Application.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Experts.Queries.GetExpertProfileByExpertId;

public record GetExpertProfileByExpertIdQuery(int ExpertId)
    : IRequest<Result<ExpertProfileDto>>;
