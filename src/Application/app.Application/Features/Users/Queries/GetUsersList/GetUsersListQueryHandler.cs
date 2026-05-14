using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Services.UserAgg;
using app.Application.Contracts.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Users.Queries.GetUsersList;

public class GetUsersListQueryHandler(IUserService userService) : IRequestHandler<GetUsersListQuery, Result<List<UserSummaryDto>>>
{
    public async Task<Result<List<UserSummaryDto>>> Handle(GetUsersListQuery request, CancellationToken ct)
    {
        var users = await userService.GetAllAsync(request.Search, ct);
        return Result<List<UserSummaryDto>>.Success(users);
    }
}