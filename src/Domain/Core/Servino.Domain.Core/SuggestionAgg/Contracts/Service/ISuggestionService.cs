using Servino.Domain.Core.SuggestionAgg.Dtos;

namespace Servino.Domain.Core.SuggestionAgg.Contracts.Service;

public interface ISuggestionService
{
    Task<List<SuggestionSummaryDto>> GetByRequestIdAsync(int requestId, CancellationToken ct);
    Task<SuggestionDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<bool> UpdateAsync(UpdateSuggestionDto command, CancellationToken ct);
}