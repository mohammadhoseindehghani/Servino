using System.Security.Claims;

namespace Servino.Presentation.RazorPagesUI.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal principal)
    {
        var value = principal.FindFirstValue("UserId");
        return int.TryParse(value, out var id) ? id : 0;
    }

    public static int GetCustomerId(this ClaimsPrincipal principal)
    {
        var value = principal.FindFirstValue("CustomerId");
        return int.TryParse(value, out var id) ? id : 0;
    }

    public static int GetExpertId(this ClaimsPrincipal principal)
    {
        var value = principal.FindFirstValue("ExpertId");
        return int.TryParse(value, out var id) ? id : 0;
    }
}