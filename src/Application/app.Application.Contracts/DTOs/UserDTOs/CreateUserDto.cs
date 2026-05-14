namespace app.Application.Contracts.DTOs.UserDTOs;

public record CreateUserDto
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Mobile { get; init; }
    public string IdentityId { get; init; }
    public int? CityId { get; init; }
}