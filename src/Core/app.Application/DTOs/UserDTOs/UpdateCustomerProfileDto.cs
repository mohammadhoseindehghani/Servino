namespace app.Application.DTOs.UserDTOs;

public record UpdateCustomerProfileDto
{
    public int UserId { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public int? CityId { get; init; }
    public string? ProfileImagePath { get; init; }
}