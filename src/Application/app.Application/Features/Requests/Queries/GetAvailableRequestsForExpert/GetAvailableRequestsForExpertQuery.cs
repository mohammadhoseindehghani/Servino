using app.Application.Contracts.DTOs.RequestDTOs;
using MediatR;

namespace app.Application.Features.Requests.Queries.GetAvailableRequestsForExpert;

public record GetAvailableRequestsForExpertQuery(int ExpertId)
    : IRequest<List<RequestSummaryDto>>;
