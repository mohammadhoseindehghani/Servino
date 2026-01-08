using Servino.Domain.Core.SuggestionAgg.Contracts.AppService;
using Servino.Domain.Core.SuggestionAgg.Contracts.Service;
using Servino.Domain.Core.SuggestionAgg.Dtos;

namespace Servino.Domain.AppService;

public class SuggestionAppService(ISuggestionService suggestionService) : ISuggestionAppService
{
    public async Task<List<SuggestionSummaryDto>> GetByRequestIdAsync(int requestId, CancellationToken ct)
    {
        return await suggestionService.GetByRequestIdAsync(requestId, ct);
    }
}