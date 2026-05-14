namespace app.Application.Contracts.DTOs.IdentityDTOs;

public record SendOtpDto
{
    public string MobileNumber { get; init; }
}