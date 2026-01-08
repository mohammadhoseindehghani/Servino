using Servino.Domain.Core.SuggestionAgg.Dtos;

namespace Servino.Domain.Core.SuggestionAgg.Contracts.AppService;

public interface ISuggestionAppService
{
    Task<List<SuggestionSummaryDto>> GetByRequestIdAsync(int requestId, CancellationToken ct);
}