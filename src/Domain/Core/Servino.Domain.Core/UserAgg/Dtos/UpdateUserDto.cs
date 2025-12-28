namespace Servino.Domain.Core.UserAgg.Dtos;

public class UpdateUserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Mobile { get; set; } 
    public int? CityId { get; set; }
    public string? ProfileImagePath { get; set; }
}