namespace Servino.Domain.Core.UserAgg.Dtos;

public class UpdateCustomerProfileDto
{
    public int UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? CityId { get; set; }
    public string? ProfileImagePath { get; set; }
}