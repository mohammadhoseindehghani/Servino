using Servino.Domain.Core._common;
using Servino.Domain.Core.RequestAgg.Entity;
using Servino.Domain.Core.SuggestionAgg.Enum;
using Servino.Domain.Core.UserAgg.Entity;

namespace Servino.Domain.Core.SuggestionAgg.Entity;

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