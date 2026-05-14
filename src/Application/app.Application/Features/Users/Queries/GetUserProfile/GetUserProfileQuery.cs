using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Users.Queries.GetUserProfile;

public record GetUserProfileQuery(int UserId)
    : IRequest<Result<UserDetailDto>>;
