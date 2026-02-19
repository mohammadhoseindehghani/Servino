namespace Servino.Domain.Core.UserAgg.Dtos.Identity;

public class LoginWithOtpDto
{
    public string MobileNumber { get; set; }
    public string Code { get; set; }
}