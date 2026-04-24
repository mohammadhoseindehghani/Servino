using app.Application.DTOs.Common;

namespace app.Application.DTOs.LocationDTOs;

public record ProvinceDto : BaseDto
{
    public string Title { get; set; }
    public int CityCount { get; set; } 
}