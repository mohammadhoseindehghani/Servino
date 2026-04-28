using app.Application.DTOs.Common;
using app.Domain.SuggestionAgg.Enums;

namespace app.Application.DTOs.SuggestionDTOs;

public record SuggestionSummaryDto : BaseDto
{
    public int ExpertId { get; init; }
    public string ExpertFullName { get; init; } 
    public string? ExpertMobile { get; init; }  
    public decimal SuggestedPrice { get; init; }
    public DateTime SuggestedDate { get; init; }
    public int EstimatedDurationHours { get; init; }
    public string? Note { get; init; }
    public SuggestionStatus Status { get; init; }
    public DateTime CreatedAt { get; init; } 
}