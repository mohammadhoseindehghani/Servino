using app.Application.Common;
using app.Application.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Experts.Queries.GetExpertProfileByUserId;

public record GetExpertProfileByUserIdQuery(int UserId)
    : IRequest<Result<ExpertProfileDto>>;
