using app.Application.DTOs.Common;

namespace app.Application.DTOs.LocationDTOs;

public record CityDto : BaseDto
{
    public string Title { get; init; }
    public int ProvinceId { get; init; }
    public string ProvinceName { get; init; }
}