namespace app.Application.Contracts.DTOs.IdentityDTOs;

public record LoginWithOtpDto
{
    public string MobileNumber { get; init; }
    public string Code { get; init; }
}