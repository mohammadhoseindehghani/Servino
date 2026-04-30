using app.Application.Contracts.Repositories;
using app.Application.DTOs.RequestDTOs;
using MediatR;

namespace app.Application.Features.Requests.Queries.GetAllRequestsQuery;

public class GetAllRequestsQueryHandler(IRequestRepository requestRepository)
    : IRequestHandler<GetAllRequestsQuery, List<RequestSummaryDto>>
{

    public async Task<List<RequestSummaryDto>> Handle(GetAllRequestsQuery query, CancellationToken ct)
    {
        return await requestRepository.GetAllAsync(query.Search, query.CategoryId, query.CityId, ct);
    }
}
