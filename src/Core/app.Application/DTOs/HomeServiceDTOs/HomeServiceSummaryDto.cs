using app.Application.DTOs.Common;

namespace app.Application.DTOs.HomeServiceDTOs;

public record HomeServiceSummaryDto : BaseDto
{
    public string Title { get; init; }
    public string CategoryName { get; init; } 
    public decimal BasePrice { get; init; } 
    public int VisitCount { get; init; }
    public string? ImagePath { get; init; }
}