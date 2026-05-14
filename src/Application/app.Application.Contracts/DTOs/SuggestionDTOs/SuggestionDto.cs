using app.Application.Contracts.DTOs.Common;
using app.Domain.SuggestionAgg.Enums;

namespace app.Application.Contracts.DTOs.SuggestionDTOs;

public record SuggestionDto : BaseDto
{
    public int RequestId { get; init; }
    public int ExpertId { get; init; }
    public int ExpertUserId { get; init; }
    public decimal SuggestedPrice { get; init; }
    public DateTime SuggestedDate { get; init; }
    public int EstimatedDurationHours { get; init; }
    public string? Note { get; init; }
    public SuggestionStatus Status { get; set; }
    public DateTime CreatedAt { get; init; }
}