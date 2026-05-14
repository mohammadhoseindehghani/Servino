namespace app.Application.Contracts.DTOs.IdentityDTOs;

public record RegisterDto
{
    public string UserName { get; init; } 
    public string Email { get; init; }
    public string PhoneNumber { get; init; }
    public string Password { get; init; }
    public string Role { get; init; } 
}