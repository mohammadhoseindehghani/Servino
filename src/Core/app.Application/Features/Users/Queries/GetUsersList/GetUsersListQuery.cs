using app.Application.Common;
using app.Application.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Users.Queries.GetUsersList;

public record GetUsersListQuery(PaginationRequestDto Search)
    : IRequest<Result<List<UserSummaryDto>>>;
