namespace app.Application.DTOs.UserDTOs;

public record AdminExpertInfoDto
{
    public string? Bio { get; init; }
    public string? Address { get; init; }
    public string? BankCardNumber { get; init; }
    public string? ShebaNumber { get; init; }
}