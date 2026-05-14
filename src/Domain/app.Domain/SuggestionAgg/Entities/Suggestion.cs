using app.Domain._common;
using app.Domain.RequestAgg.Entities;
using app.Domain.SuggestionAgg.Enums;
using app.Domain.UserAgg.Entities;

namespace app.Domain.SuggestionAgg.Entities;

public class Suggestion : BaseEntity
{
    public decimal SuggestedPrice { get; set; } 
    public DateTime SuggestedDate { get; set; } 
    public int EstimatedDurationHours { get; set; } 
    public string? Note { get; set; } 
    public SuggestionStatus Status { get; set; }

    public int ExpertId { get; set; }
    public int RequestId { get; set; }

    public Expert Expert { get; set; }
    public Request Request { get; set; }
}