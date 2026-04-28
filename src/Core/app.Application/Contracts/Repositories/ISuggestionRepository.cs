using app.Application.DTOs.SuggestionDTOs;

namespace app.Application.Contracts.Repositories;

public interface ISuggestionRepository
{ 
    Task<bool> IsExpertSendSuggestionBeforeAsync(int expertId, int requestId, CancellationToken ct);
    Task<bool> CreateAsync(CreateSuggestionDto command, CancellationToken ct);
    Task<List<SuggestionSummaryDto>> GetByRequestIdAsync(int requestId, CancellationToken ct);
    Task<SuggestionDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<bool> UpdateAsync(UpdateSuggestionDto command, CancellationToken ct);
}