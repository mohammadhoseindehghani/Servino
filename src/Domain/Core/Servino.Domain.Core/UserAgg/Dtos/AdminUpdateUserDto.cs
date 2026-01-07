namespace Servino.Domain.Core.UserAgg.Dtos;

public class AdminUpdateUserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Mobile { get; set; }
    public int CityId { get; set; }
    public string Role { get; set; } 

    public string? NewPassword { get; set; }

    public AdminExpertInfoDto? ExpertInfo { get; set; }
}