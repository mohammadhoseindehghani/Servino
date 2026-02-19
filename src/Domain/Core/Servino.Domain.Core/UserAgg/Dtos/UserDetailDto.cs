namespace Servino.Domain.Core.UserAgg.Dtos;

public class UserDetailDto : UpdateUserDto
{
    public string Email { get; set; }
    public decimal BalanceAmount { get; set; } 
    public DateTime RegisterDate { get; set; }
    public string IdentityId { get; set; }
}