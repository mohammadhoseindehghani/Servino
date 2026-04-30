using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.DTOs.IdentityDTOs;
using app.Application.DTOs.UserDTOs;
using app.Domain.UserAgg.Entities;
using MediatR;

namespace app.Application.Features.Users.Queries.GetUserProfile;

public class GetUserProfileQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetUserProfileQuery, Result<UserDetailDto>>
{

    public async Task<Result<UserDetailDto>> Handle(GetUserProfileQuery request, CancellationToken ct)
    {
        var profile = await userRepository.GetByIdAsync(request.UserId, ct);

        return profile == null
            ? Result<UserDetailDto>.Failure("کاربر یافت نشد.", "404")
            : Result<UserDetailDto>.Success(profile);
    }
}
