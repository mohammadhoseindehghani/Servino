
namespace app.Application.DTOs.SuggestionDTOs;

public record CreateSuggestionDto
{
    public decimal SuggestedPrice { get; set; }
    public DateTime SuggestedDate { get; set; }
    public int EstimatedDurationHours { get; set; }
    public string? Note { get; set; }
    public int ExpertId { get; set; }
    public int RequestId { get; set; }
}