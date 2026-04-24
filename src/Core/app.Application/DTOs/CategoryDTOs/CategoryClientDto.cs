using app.Application.DTOs.Common;

namespace app.Application.DTOs.CategoryDTOs;

public record CategoryClientDto : BaseDto
{
    public string Title { get; init; }
    public string? ImagePath { get; init; }
    public bool HasChildren { get; init; }
}