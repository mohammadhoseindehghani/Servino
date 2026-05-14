using app.Application.Contracts.DTOs.Common;

namespace app.Application.Contracts.DTOs.CategoryDTOs;

public record CategoryDto : BaseDto
{
    public string Title { get; init; }
    public string? ImagePath { get; init; }
    public int? ParentId { get; init; }
}