using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.RequestDTOs;
using MediatR;

namespace app.Application.Features.Requests.Queries.GetAllRequestsQuery;

public record GetAllRequestsQuery(
    PaginationRequestDto Search,
    int? CategoryId,
    int? CityId)
    : IRequest<List<RequestSummaryDto>>;

