namespace app.Application.DTOs.IdentityDTOs;

public record LoginWithGoogleDto
{
    public string IdToken { get; init; }
}