namespace Servino.Domain.Core.CategoryAgg.Dtos;

public class CategorySummaryDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? ParentTitle { get; set; } 
    public int SubCategoriesCount { get; set; } 
    public bool IsActive { get; set; }
    public string? ImagePath { get; set; }
}