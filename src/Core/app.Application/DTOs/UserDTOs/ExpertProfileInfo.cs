namespace app.Application.DTOs.UserDTOs;

public record ExpertProfileInfo
{
    public string? Bio { get; set; }
    public string? Address { get; set; }
    public string? BankCardNumber { get; set; }
    public string? ShebaNumber { get; set; }
    public decimal? AverageScore { get; set; }
}