using app.Application.Contracts.DTOs.Common;

namespace app.Application.Contracts.DTOs.CategoryDTOs;

public record MainCategoryDto : BaseDto
{
    public string Title { get; init; }
    public string? ImagePath { get; init; }
}