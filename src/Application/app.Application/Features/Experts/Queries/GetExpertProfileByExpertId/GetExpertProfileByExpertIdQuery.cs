using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Experts.Queries.GetExpertProfileByExpertId;

public record GetExpertProfileByExpertIdQuery(int ExpertId)
    : IRequest<Result<ExpertProfileDto>>;
