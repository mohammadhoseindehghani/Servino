namespace app.Application.Contracts.DTOs.IdentityDTOs;

public record LoginWithGoogleDto
{
    public string IdToken { get; init; }
}