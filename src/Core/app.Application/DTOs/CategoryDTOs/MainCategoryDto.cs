using app.Application.DTOs.Common;

namespace app.Application.DTOs.CategoryDTOs;

public record MainCategoryDto : BaseDto
{
    public string Title { get; init; }
    public string? ImagePath { get; init; }
}