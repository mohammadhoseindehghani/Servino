namespace app.Application.Contracts.DTOs.UserDTOs;

public record UserDetailDto : UpdateUserDto
{
    public string Email { get; init; }
    public decimal BalanceAmount { get; init; } 
    public DateTime RegisterDate { get; init; }
    public string IdentityId { get; init; }
}