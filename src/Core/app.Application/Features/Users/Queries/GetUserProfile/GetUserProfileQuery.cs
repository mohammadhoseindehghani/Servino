using app.Application.Common;
using app.Application.DTOs.IdentityDTOs;
using app.Application.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Users.Queries.GetUserProfile;

public record GetUserProfileQuery(int UserId)
    : IRequest<Result<UserDetailDto>>;
