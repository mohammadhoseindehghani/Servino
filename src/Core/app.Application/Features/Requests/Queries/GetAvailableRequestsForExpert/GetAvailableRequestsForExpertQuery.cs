using app.Application.DTOs.RequestDTOs;
using MediatR;

namespace app.Application.Features.Requests.Queries.GetAvailableRequestsForExpert;

public record GetAvailableRequestsForExpertQuery(int ExpertId)
    : IRequest<List<RequestSummaryDto>>;
