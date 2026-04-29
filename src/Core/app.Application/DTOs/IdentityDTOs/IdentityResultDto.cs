namespace app.Application.DTOs.IdentityDTOs;

public record IdentityResultDto
{
    public bool Succeeded { get; init; }
    public string? Message { get; init; }
    public string? Id { get; init; }
}