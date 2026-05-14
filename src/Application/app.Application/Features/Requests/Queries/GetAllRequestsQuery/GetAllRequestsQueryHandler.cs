using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.RequestAgg;
using app.Application.Contracts.DTOs.RequestDTOs;
using MediatR;

namespace app.Application.Features.Requests.Queries.GetAllRequestsQuery;

public class GetAllRequestsQueryHandler(IRequestService requestService)
    : IRequestHandler<GetAllRequestsQuery, List<RequestSummaryDto>>
{

    public async Task<List<RequestSummaryDto>> Handle(GetAllRequestsQuery query, CancellationToken ct)
    {
        return await requestService.GetAllAsync(query.Search, query.CategoryId, query.CityId, ct);
    }
}
