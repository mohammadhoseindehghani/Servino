using app.Application.DTOs.Common;

namespace app.Application.DTOs.UserDTOs;

public record UpdateUserDto : BaseDto
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? Mobile { get; init; } 
    public int? CityId { get; init; }
    public string? ProfileImagePath { get; init; }
    public string? Email { get; init; }
}