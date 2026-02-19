namespace Servino.Domain.Core.UserAgg.Dtos.Identity;

public class IdentityResultDto
{
    public bool Succeeded { get; set; }
    public string? Message { get; set; }
    public string? Id { get; set; }
}