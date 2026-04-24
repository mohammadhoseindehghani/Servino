namespace app.Application.DTOs.UserDTOs;

public record CustomerProfileDto
{
    public int UserId { get; init; }
    public int ExpertId { get; init; }
    public string? Email { get; init; }
    public string Phone { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public decimal Balance { get; init; }
    public int? CityId { get; init; }
    public string? ProfileImagePath { get; init; }
}