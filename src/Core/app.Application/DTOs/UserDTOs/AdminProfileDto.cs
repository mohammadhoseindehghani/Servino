namespace app.Application.DTOs.UserDTOs;

public record AdminProfileDto
{
    public int UserId { get; init; }
    public int AdminId { get; init; }
    public string? Email { get; init; }
    public string Phone { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public decimal Balance { get; init; }
    public string CityName { get; init; }
    public string? ProfileImagePath { get; init; }
    public DateTime RegisterDate { get; init; }
}