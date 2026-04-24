using app.Application.DTOs.Common;
using app.Domain.Enums;

namespace app.Application.DTOs.SuggestionDTOs;

public record SuggestionDto : BaseDto
{
    public int RequestId { get; init; }
    public int ExpertId { get; init; }
    public int ExpertUserId { get; init; }
    public decimal SuggestedPrice { get; init; }
    public DateTime SuggestedDate { get; init; }
    public int EstimatedDurationHours { get; init; }
    public string? Note { get; init; }
    public SuggestionStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
}