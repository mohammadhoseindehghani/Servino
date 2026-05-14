namespace app.Application.Contracts.DTOs.IdentityDTOs;

public record IdentityResultDto
{
    public bool Succeeded { get; init; }
    public string? Message { get; init; }
    public string? Id { get; init; }
}