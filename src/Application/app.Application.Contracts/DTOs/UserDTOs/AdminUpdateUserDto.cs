using app.Application.Contracts.DTOs.Common;

namespace app.Application.Contracts.DTOs.UserDTOs;

public record AdminUpdateUserDto : BaseDto
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Mobile { get; init; }
    public int CityId { get; init; }
    public int ProvinceId { get; init; }
    public string Role { get; init; }
    public string? ProfileImagePath { get; init; }

    public string? NewPassword { get; init; }

    public AdminExpertInfoDto? ExpertInfo { get; init; }
}