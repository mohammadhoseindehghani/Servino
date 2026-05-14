using System.Security.Claims;

namespace app.Application.Contracts.Contracts.Providers_Services;


public interface IJwtTokenService
{
    string GenerateToken(ClaimsPrincipal principal);
}
