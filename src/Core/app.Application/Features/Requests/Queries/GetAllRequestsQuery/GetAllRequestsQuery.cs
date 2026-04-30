using app.Application.Common;
using app.Application.DTOs.RequestDTOs;
using MediatR;

namespace app.Application.Features.Requests.Queries.GetAllRequestsQuery;

public record GetAllRequestsQuery(
    PaginationRequestDto Search,
    int? CategoryId,
    int? CityId)
    : IRequest<List<RequestSummaryDto>>;

