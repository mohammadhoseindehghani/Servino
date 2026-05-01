using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.DTOs.IdentityDTOs;
using app.Application.DTOs.UserDTOs;
using app.Domain.UserAgg.Entities;
using FluentValidation;
using MediatR;

namespace app.Application.Features.Users.Queries.GetUserProfile;

public class GetUserProfileQueryHandler(IUserRepository userRepository, IValidator<GetUserProfileQuery> validator)
    : IRequestHandler<GetUserProfileQuery, Result<UserDetailDto>>
{

    public async Task<Result<UserDetailDto>> Handle(GetUserProfileQuery request, CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var profile = await userRepository.GetByIdAsync(request.UserId, ct);

        return profile == null
            ? Result<UserDetailDto>.Failure("کاربر یافت نشد.", "404")
            : Result<UserDetailDto>.Success(profile);
    }
}
