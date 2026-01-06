using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Servino.Domain.Core.UserAgg.Contracts.Service;
using System.Security.Claims;
using Servino.Domain.Core.UserAgg.Contracts.Data;

namespace Servino.Infa.Db.SqlServer.EfCore.Identity.Service;

public class AppUserClaimsPrincipalFactory(
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IOptions<IdentityOptions> optionsAccessor,
    IUserService userService,
    ICustomerRepository customerRepository, 
    IExpertRepository expertRepository      
)
    : UserClaimsPrincipalFactory<IdentityUser, IdentityRole>(userManager, roleManager, optionsAccessor)
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(IdentityUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        var appUserId = await userService.GetIdByIdentityIdAsync(user.Id, CancellationToken.None);
        identity.AddClaim(new Claim("UserId", appUserId.ToString()));

        var customerId = await customerRepository.GetCustomerIdByUserIdAsync(appUserId, CancellationToken.None);
        if (customerId > 0)
        {
            identity.AddClaim(new Claim("CustomerId", customerId.ToString()));
        }

        var expertId = await expertRepository.GetIdByUserIdAsync(appUserId, CancellationToken.None);
        if (expertId > 0)
        {
            identity.AddClaim(new Claim("ExpertId", expertId.ToString()));
        }

        return identity;
    }
}