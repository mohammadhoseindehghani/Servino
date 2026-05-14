using app.Application.Contracts.DTOs.UserDTOs;

namespace app.Application.Contracts.DTOs.IdentityDTOs;

public record UserProfileDto
{
    public int Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; } 
    public string MobileNumber { get; init; } 
    public int? CityId { get; init; }
    public string? CityName { get; init; }
    public string? ProfileImagePath { get; init; }
    public decimal Balance { get; init; }
    public DateTime RegisterDate { get; init; }
    public string Role { get; init; } 

    public ExpertProfileInfo? ExpertInfo { get; init; } 

}