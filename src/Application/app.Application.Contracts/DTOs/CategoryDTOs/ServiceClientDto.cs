using app.Application.Contracts.DTOs.Common;

namespace app.Application.Contracts.DTOs.CategoryDTOs;

public record ServiceClientDto : BaseDto
{
    public string Title { get; init; }
    public string? ShortDescription { get; init; }
    public decimal BasePrice { get; init; }
    public string? ImagePath { get; init; }
}