using Servino.Domain.Core.RequestAgg.Enum;

namespace Servino.Domain.Core.RequestAgg.Dtos;

public class RequestSummaryDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ServiceName { get; set; } 
    public string CityName { get; set; }
    public RequestStatus Status { get; set; } 
    public DateTime DateRequired { get; set; }
    public DateTime CreatedAt { get; set; }
    public int SuggestionCount { get; set; }

    public bool HasExpertSuggestion { get; set; }
    public int? ExpertSuggestionId { get; set; }
}