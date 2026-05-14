using app.Application.Contracts.DTOs.Common;

namespace app.Application.Contracts.DTOs.LocationDTOs;

public record SelectListDto : BaseDto
{ 
    public string Title { get; init; }
}