namespace Servino.Domain.Core.UserAgg.Dtos.Identity;

public class LoginResultDto
{
    public bool Succeeded { get; set; }
    public string? Message { get; set; }
    public string? Token { get; set; } 
    public string? RefreshToken { get; set; }
}