using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Servino.Domain.Core.UserAgg.Contracts.Service;
using System.Security.Claims;

namespace Servino.Infa.Db.SqlServer.EfCore.Identity.Service;

public class AppUserClaimsPrincipalFactory(
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IOptions<IdentityOptions> optionsAccessor,
    IUserService userService)
    : UserClaimsPrincipalFactory<IdentityUser, IdentityRole>(userManager, roleManager, optionsAccessor)
{

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(IdentityUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        var appUserId = await userService.GetIdByIdentityIdAsync(user.Id, CancellationToken.None);

        identity.AddClaim(new Claim("userId", appUserId.ToString()));

        return identity;
    }
}