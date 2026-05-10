using System.Security.Claims;

namespace app.Application.Contracts.Services;


public interface IJwtTokenService
{
    string GenerateToken(ClaimsPrincipal principal);
}
