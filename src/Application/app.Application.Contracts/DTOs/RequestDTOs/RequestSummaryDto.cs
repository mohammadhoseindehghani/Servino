using app.Application.Contracts.DTOs.Common;
using app.Domain.SuggestionAgg.Enums;

namespace app.Application.Contracts.DTOs.RequestDTOs;

public record RequestSummaryDto : BaseDto
{
    public string Title { get; init; }
    public string ServiceName { get; init; } 
    public string CityName { get; init; }
    public RequestStatus Status { get; init; } 
    public DateTime DateRequired { get; init; }
    public DateTime CreatedAt { get; init; }
    public int SuggestionCount { get; init; }

    public bool HasExpertSuggestion { get; init; }
    public int? ExpertSuggestionId { get; init; }
}