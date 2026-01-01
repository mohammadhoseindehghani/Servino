namespace Servino.Domain.Core.UserAgg.Dtos;

public class UpdateProfileDto
{
    public int Id { get; set; } 
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int? CityId { get; set; }
    public string? ProfileImagePath { get; set; } 


    public string? Bio { get; set; }
    public string? Address { get; set; }
    public string? BankCardNumber { get; set; }
    public string? ShebaNumber { get; set; }
}