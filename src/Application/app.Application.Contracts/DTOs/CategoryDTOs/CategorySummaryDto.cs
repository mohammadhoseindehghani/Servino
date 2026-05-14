using app.Application.Contracts.DTOs.Common;

namespace app.Application.Contracts.DTOs.CategoryDTOs;

public record CategorySummaryDto : BaseDto
{
    public string Title { get; init; }
    public string? ParentTitle { get; init; } 
    public int SubCategoriesCount { get; init; } 
    public bool IsActive { get; init; }
    public string? ImagePath { get; init; }
}