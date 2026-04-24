using app.Application.DTOs.Common;

namespace app.Application.DTOs.CategoryDTOs;

public record BreadcrumbDto : BaseDto
{
    public string Title { get; init; }
    public int? ParentId { get; init; }
}