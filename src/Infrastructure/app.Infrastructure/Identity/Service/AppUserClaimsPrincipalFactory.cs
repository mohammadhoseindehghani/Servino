using System.Security.Claims;
using app.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace app.Infrastructure.Identity.Service;

public class AppUserClaimsPrincipalFactory(
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IOptions<IdentityOptions> optionsAccessor,
    IUserRepository userRepository,
    ICustomerRepository customerRepository, 
    IExpertRepository expertRepository      
)
    : UserClaimsPrincipalFactory<IdentityUser, IdentityRole>(userManager, roleManager, optionsAccessor)
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(IdentityUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        var appUserId = await userRepository.GetIdByIdentityIdAsync(user.Id, CancellationToken.None);
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