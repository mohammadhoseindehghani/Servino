namespace Servino.Domain.Core.UserAgg.Dtos;

public class AdminProfileDto
{
    public int UserId { get; set; }
    public int AdminId { get; set; }
    public string? Email { get; set; }
    public string Phone { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public decimal Balance { get; set; }
    public string CityName { get; set; }
    public string? ProfileImagePath { get; set; }
    public DateTime RegisterDate { get; set; }
}