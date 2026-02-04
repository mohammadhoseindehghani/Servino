using Servino.Domain.Core.SuggestionAgg.Enum;

namespace Servino.Domain.Core.SuggestionAgg.Dtos;

public class SuggestionDto
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public int ExpertId { get; set; }
    public int ExpertUserId { get; set; }
    public decimal SuggestedPrice { get; set; }
    public DateTime SuggestedDate { get; set; }
    public int EstimatedDurationHours { get; set; }
    public string? Note { get; set; }
    public SuggestionStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}