namespace app.Application.DTOs.IdentityDTOs;

public record LoginWithPassDto
{
    public string UserName { get; init; }
    public string Password { get; init; }
}