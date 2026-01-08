using Servino.Domain.Core.SuggestionAgg.Enum;

namespace Servino.Domain.Core.SuggestionAgg.Dtos;

public class SuggestionSummaryDto
{
    public int Id { get; set; }
    public int ExpertId { get; set; }
    public string ExpertFullName { get; set; } 
    public string? ExpertMobile { get; set; }  
    public decimal SuggestedPrice { get; set; }
    public DateTime SuggestedDate { get; set; }
    public int EstimatedDurationHours { get; set; }
    public string? Note { get; set; }
    public SuggestionStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } 
}