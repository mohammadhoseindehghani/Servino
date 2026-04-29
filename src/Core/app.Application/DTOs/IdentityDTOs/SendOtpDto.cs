namespace app.Application.DTOs.IdentityDTOs;

public record SendOtpDto
{
    public string MobileNumber { get; init; }
}