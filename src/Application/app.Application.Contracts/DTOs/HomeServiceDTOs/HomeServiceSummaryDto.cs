using app.Application.Contracts.DTOs.Common;

namespace app.Application.Contracts.DTOs.HomeServiceDTOs;

public record HomeServiceSummaryDto : BaseDto
{
    public string Title { get; init; }
    public string CategoryName { get; init; } 
    public decimal BasePrice { get; init; } 
    public int VisitCount { get; init; }
    public string? ImagePath { get; init; }
}