namespace Servino.Domain.Core.UserAgg.Dtos;

public class UserSummaryDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public decimal Balance { get; set; } 
    public DateTime RegisterDate { get; set; } 
    public bool IsActive { get; set; }
    public string CityName { get; set; }
    public string? ImageUrl { get; set; }
    public string Role { get; set; }

    public int? ProvinceId { get; set; }
    public int? CityId { get; set; }
}