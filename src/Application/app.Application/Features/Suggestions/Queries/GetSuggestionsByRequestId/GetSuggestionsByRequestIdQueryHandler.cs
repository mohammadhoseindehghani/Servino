using app.Application.Contracts.Contracts.Services.Suggestion;
using app.Application.Contracts.DTOs.SuggestionDTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Suggestions.Queries.GetSuggestionsByRequestId;

public class GetSuggestionsByRequestIdQueryHandler(
    ISuggestionService suggestionService,
    ILogger<GetSuggestionsByRequestIdQueryHandler> logger)
    : IRequestHandler<GetSuggestionsByRequestIdQuery, List<SuggestionSummaryDto>>
{
    public async Task<List<SuggestionSummaryDto>> Handle(
        GetSuggestionsByRequestIdQuery query,
        CancellationToken ct)
    {
        try
        {
            return await suggestionService.GetByRequestIdAsync(query.RequestId, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "System error in SuggestionAppService.GetByRequestIdAsync | RequestId: {RequestId}",
                query.RequestId);

            return new List<SuggestionSummaryDto>();
        }
    }
}
