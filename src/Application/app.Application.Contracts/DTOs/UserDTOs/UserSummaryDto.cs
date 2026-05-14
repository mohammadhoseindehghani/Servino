using app.Application.Contracts.DTOs.Common;

namespace app.Application.Contracts.DTOs.UserDTOs;

public record UserSummaryDto : BaseDto
{
    public string FullName { get; init; }
    public string Email { get; init; }
    public string Mobile { get; init; }
    public decimal Balance { get; init; } 
    public DateTime RegisterDate { get; init; } 
    public bool IsActive { get; init; }
    public string CityName { get; init; }
    public string? ImageUrl { get; init; }
    public string Role { get; init; }

    public int? ProvinceId { get; init; }
    public int? CityId { get; init; }
}