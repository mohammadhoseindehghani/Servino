using app.Application.DTOs.Common;

namespace app.Application.DTOs.UserDTOs;

public record UpdateProfileDto : BaseDto
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public int? CityId { get; init; }
    public string? ProfileImagePath { get; init; } 


    public string? Bio { get; init; }
    public string? Address { get; init; }
    public string? BankCardNumber { get; init; }
    public string? ShebaNumber { get; init; }
}