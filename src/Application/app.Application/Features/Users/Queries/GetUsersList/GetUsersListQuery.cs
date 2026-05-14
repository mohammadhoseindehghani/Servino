using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Users.Queries.GetUsersList;

public record GetUsersListQuery(PaginationRequestDto Search)
    : IRequest<Result<List<UserSummaryDto>>>;
