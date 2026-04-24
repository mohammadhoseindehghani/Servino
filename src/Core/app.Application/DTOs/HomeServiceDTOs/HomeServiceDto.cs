using app.Application.DTOs.Common;

namespace app.Application.DTOs.HomeServiceDTOs;

public record HomeServiceDto : BaseDto
{
    public string Title { get; init; }
    public decimal BasePrice { get; init; }
    public int CategoryId { get; init; }
    public string? ShortDescription { get; init; }
    public string? ImagePath { get; init; }
}