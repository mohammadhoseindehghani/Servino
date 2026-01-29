using System.Collections.Specialized;
using System.Security.AccessControl;

namespace Servino.Domain.Core.UserAgg.Dtos;

public class UserSummaryProjectionDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string MobileNumber { get; set; }
    public string? CityTitle { get; set; }
    public decimal Balance { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ProfileImagePath { get; set; }
    public bool HasAdmin { get; set; }
    public bool HasExpert { get; set; }
    public bool HasCustomer { get; set; }

    public int? ProvinceId { get; set; }
    public int? CityId { get; set; }
}