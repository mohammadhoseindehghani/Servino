using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Experts.Queries.GetExpertProfileByUserId;

public record GetExpertProfileByUserIdQuery(int UserId)
    : IRequest<Result<ExpertProfileDto>>;
