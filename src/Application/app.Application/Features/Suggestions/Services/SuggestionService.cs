using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.Suggestion;
using app.Application.Contracts.DTOs.SuggestionDTOs;

namespace app.Application.Features.Suggestions.Services;

public class SuggestionService(ISuggestionRepository suggestionRepo) : ISuggestionService
{
    public async Task<bool> IsExpertSendSuggestionBeforeAsync(int expertId, int requestId, CancellationToken ct)
    {
        return await suggestionRepo.IsExpertSendSuggestionBeforeAsync(expertId, requestId, ct);
    }

    public async Task<bool> CreateAsync(CreateSuggestionDto command, CancellationToken ct)
    {
        return await suggestionRepo.CreateAsync(command, ct);
    }

    public async Task<List<SuggestionSummaryDto>> GetByRequestIdAsync(int requestId, CancellationToken ct)
    {
        return await suggestionRepo.GetByRequestIdAsync(requestId, ct);
    }

    public async Task<SuggestionDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await suggestionRepo.GetByIdAsync(id, ct);
    }

    public async Task<bool> UpdateAsync(UpdateSuggestionDto command, CancellationToken ct)
    {
        return await suggestionRepo.UpdateAsync(command, ct);
    }
}