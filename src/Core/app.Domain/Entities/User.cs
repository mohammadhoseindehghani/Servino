using app.Domain.Common;

namespace app.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string MobileNumber { get; set; }
    public int? CityId { get; set; }
    public City? City { get; set; }
    public string? ProfileImagePath { get; set; } 
    public decimal Balance { get; set; } 
    public bool IsActive { get; set; }

    public string IdentityId { get; set; }

    public Customer? Customer { get; set; }
    public Expert? Expert { get; set; }
    public Admin? Admin { get; set; }
}