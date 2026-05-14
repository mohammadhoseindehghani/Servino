namespace app.Application.Contracts.DTOs.IdentityDTOs;

public record LoginWithPassDto
{
    public string UserName { get; init; }
    public string Password { get; init; }
}