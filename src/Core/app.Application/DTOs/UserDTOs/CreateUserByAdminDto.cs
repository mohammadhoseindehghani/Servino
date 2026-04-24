namespace app.Application.DTOs.UserDTOs;

public record CreateUserByAdminDto
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Mobile { get; init; }
    public string Password { get; init; }
    public string Role { get; init; } 
}