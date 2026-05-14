using app.Application.Contracts.DTOs.RequestDTOs;
using MediatR;

namespace app.Application.Features.Requests.Queries.GetCustomerRequests;

public record GetCustomerRequestsQuery(int CustomerId)
    : IRequest<List<RequestSummaryDto>>;
