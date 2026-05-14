using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.RequestAgg;
using app.Application.Contracts.DTOs.RequestDTOs;
using MediatR;

namespace app.Application.Features.Requests.Queries.GetCustomerRequests;

public class GetCustomerRequestsQueryHandler(IRequestService requestService)
    : IRequestHandler<GetCustomerRequestsQuery, List<RequestSummaryDto>>
{

    public async Task<List<RequestSummaryDto>> Handle(GetCustomerRequestsQuery query, CancellationToken ct)
    {
        return await requestService.GetByCustomerIdAsync(query.CustomerId, ct);
    }
}
