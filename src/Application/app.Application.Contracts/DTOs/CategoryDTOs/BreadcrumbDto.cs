using app.Application.Contracts.DTOs.Common;

namespace app.Application.Contracts.DTOs.CategoryDTOs;

public record BreadcrumbDto : BaseDto
{
    public string Title { get; init; }
    public int? ParentId { get; init; }
}