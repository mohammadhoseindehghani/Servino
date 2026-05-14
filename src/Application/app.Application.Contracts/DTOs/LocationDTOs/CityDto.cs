using app.Application.Contracts.DTOs.Common;

namespace app.Application.Contracts.DTOs.LocationDTOs;

public record CityDto : BaseDto
{
    public string Title { get; init; }
    public int ProvinceId { get; init; }
    public string ProvinceName { get; init; }
}