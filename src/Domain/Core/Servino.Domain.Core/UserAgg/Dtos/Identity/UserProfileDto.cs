namespace Servino.Domain.Core.UserAgg.Dtos.Identity;

public class UserProfileDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; } 
    public string MobileNumber { get; set; } 
    public int? CityId { get; set; }
    public string? CityName { get; set; }
    public string? ProfileImagePath { get; set; }
    public decimal Balance { get; set; }
    public DateTime RegisterDate { get; set; }
    public string Role { get; set; } 

    public ExpertProfileInfo? ExpertInfo { get; set; } 

}