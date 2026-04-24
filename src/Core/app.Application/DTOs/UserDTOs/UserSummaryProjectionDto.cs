using app.Application.DTOs.Common;

namespace app.Application.DTOs.UserDTOs;

public record UserSummaryProjectionDto : BaseDto
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string MobileNumber { get; init; }
    public string? CityTitle { get; init; }
    public decimal Balance { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public string? ProfileImagePath { get; init; }
    public bool HasAdmin { get; init; }
    public bool HasExpert { get; init; }
    public bool HasCustomer { get; init; }

    public int? ProvinceId { get; init; }
    public int? CityId { get; init; }
}