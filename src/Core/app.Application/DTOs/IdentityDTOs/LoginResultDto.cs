namespace app.Application.DTOs.IdentityDTOs;

public record LoginResultDto
{
    public bool Succeeded { get; init; }
    public string? Message { get; init; }
    public string? Token { get; init; } 
    public string? RefreshToken { get; init; }
    public string Role { get; init; }
}