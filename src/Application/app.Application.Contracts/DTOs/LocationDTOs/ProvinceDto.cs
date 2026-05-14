using app.Application.Contracts.DTOs.Common;

namespace app.Application.Contracts.DTOs.LocationDTOs;

public record ProvinceDto : BaseDto
{
    public string Title { get; set; }
    public int CityCount { get; set; } 
}