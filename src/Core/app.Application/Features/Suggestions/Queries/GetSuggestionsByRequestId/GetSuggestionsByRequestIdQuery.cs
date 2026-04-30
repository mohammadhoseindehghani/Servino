using app.Application.DTOs.SuggestionDTOs;
using MediatR;

namespace app.Application.Features.Suggestions.Queries.GetSuggestionsByRequestId;

public record GetSuggestionsByRequestIdQuery(int RequestId)
    : IRequest<List<SuggestionSummaryDto>>;
