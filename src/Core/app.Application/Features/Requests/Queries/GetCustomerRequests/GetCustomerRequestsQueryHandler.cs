using app.Application.Contracts.Repositories;
using app.Application.DTOs.RequestDTOs;
using MediatR;

namespace app.Application.Features.Requests.Queries.GetCustomerRequests;

public class GetCustomerRequestsQueryHandler(IRequestRepository requestRepository)
    : IRequestHandler<GetCustomerRequestsQuery, List<RequestSummaryDto>>
{

    public async Task<List<RequestSummaryDto>> Handle(GetCustomerRequestsQuery query, CancellationToken ct)
    {
        return await requestRepository.GetByCustomerIdAsync(query.CustomerId, ct);
    }
}
