namespace app.Application.DTOs.UserDTOs;

public record UpdateExpertProfileDto
{
    public int UserId { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public int? CityId { get; init; }
    public string? ProfileImagePath { get; init; }


    public string? Bio { get; init; }
    public string? Address { get; init; }
    public string? BankCardNumber { get; init; }
    public string? ShebaNumber { get; init; }
}