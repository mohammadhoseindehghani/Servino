namespace Servino.Domain.Core.UserAgg.Dtos;

public class CreateUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string IdentityId { get; set; }
    public int? CityId { get; set; }
}