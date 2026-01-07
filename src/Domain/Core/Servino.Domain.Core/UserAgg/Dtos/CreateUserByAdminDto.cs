using System.ComponentModel.DataAnnotations;

namespace Servino.Domain.Core.UserAgg.Dtos;

public class CreateUserByAdminDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } 
}