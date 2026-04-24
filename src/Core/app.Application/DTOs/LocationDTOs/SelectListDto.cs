using app.Application.DTOs.Common;

namespace app.Application.DTOs.LocationDTOs;

public record SelectListDto : BaseDto
{ 
    public string Title { get; init; }
}