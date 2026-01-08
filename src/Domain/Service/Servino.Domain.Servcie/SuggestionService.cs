using Servino.Domain.Core.SuggestionAgg.Contracts.Data;
using Servino.Domain.Core.SuggestionAgg.Contracts.Service;
using Servino.Domain.Core.SuggestionAgg.Dtos;

namespace Servino.Domain.Service;

public class SuggestionService(ISuggestionRepository suggestionRepo) : ISuggestionService
{
    public async Task<List<SuggestionSummaryDto>> GetByRequestIdAsync(int requestId, CancellationToken ct)
    {
        return await suggestionRepo.GetByRequestIdAsync(requestId, ct);
    }
}