using Servino.Domain.Core._common;
using Servino.Domain.Core.SuggestionAgg.Dtos;

namespace Servino.Domain.Core.SuggestionAgg.Contracts.AppService;

public interface ISuggestionAppService
{
    Task<Result<bool>> CreateAsync(CreateSuggestionDto command, CancellationToken ct);
    Task<List<SuggestionSummaryDto>> GetByRequestIdAsync(int requestId, CancellationToken ct);
}