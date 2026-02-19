using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Servino.Domain.Core.LocationAgg.Entity;
using Servino.Domain.Core.RequestAgg.Enum;
using Servino.Domain.Core.SuggestionAgg.Enum;
using Servino.Domain.Core.UserAgg.Entity;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;
using Servino.Infa.Db.SqlServer.EfCore.LookUps;

namespace Servino.Infa.Db.SqlServer.EfCore.DataSeed;

public class DbInitializer(
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    AppDbContext context,
    ILogger<DbInitializer> logger)
{

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Starting database initialization...");

            await context.Database.MigrateAsync(cancellationToken);

            await SeedEnumsAsync(cancellationToken);
            await SeedRolesAsync(cancellationToken);
            await SeedLocationsAsync(cancellationToken);
            await SeedServicesAsync(cancellationToken);
            await SeedUsersAsync(cancellationToken);

            logger.LogInformation("Database initialization completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during database initialization.");
            throw; 
        }
    }

    private async Task SeedEnumsAsync(CancellationToken ct)
    {
        if (!await context.RequestStatuses.AnyAsync(ct))
        {
            foreach (var item in Enum.GetValues<RequestStatus>())
            {
                context.RequestStatuses.Add(new RequestStatusLookup
                {
                    Id = (int)item,
                    Title = item.ToString() 
                });
            }
        }

        if (!await context.SuggestionStatuses.AnyAsync(ct))
        {
            foreach (var item in Enum.GetValues<SuggestionStatus>())
            {
                context.SuggestionStatuses.Add(new SuggestionStatusLookup
                {
                    Id = (int)item,
                    Title = item.ToString()
                });
            }
        }

        await context.SaveChangesAsync(ct);
    }

    private async Task SeedRolesAsync(CancellationToken ct)
    {
        string[] roles = { "Admin", "Expert", "Customer" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private async Task SeedLocationsAsync(CancellationToken ct)
    {
        if (await context.Provinces.AnyAsync(ct))
            return;

        foreach (var provinceData in IranLocationData.Provinces)
        {
            var province = new Province { Title = provinceData.Key };

            foreach (var cityTitle in provinceData.Value)
            {
                province.Cities.Add(new City { Title = cityTitle });
            }

            context.Provinces.Add(province);
        }

        await context.SaveChangesAsync(ct);
    }

    private async Task SeedServicesAsync(CancellationToken ct)
    {
        if (await context.Categories.AnyAsync(ct))
            return;

        var allCategories = ServiceData.GetCategories();
        context.Categories.AddRange(allCategories);
        await context.SaveChangesAsync(ct);
    }

    private async Task SeedUsersAsync(CancellationToken ct)
    {
        await CreateAdminAsync("admin@servino.com", "مدیر", "سیستم", "09120000000", ct);
        await CreateExpertAsync("expert@servino.com", "اکسپرت", "سیستم", "09130000000", ct);
        await CreateCustomerAsync("customer@servino.com", "علی", "مشتری", "09121111111", ct);
    }

    private async Task CreateAdminAsync(string email, string firstName, string lastName, string mobile, CancellationToken ct)
    {
        if (await userManager.Users.AnyAsync(u => u.Email == email, ct))
            return;

        var identityUser = new IdentityUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(identityUser, "123456");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(identityUser, "Admin");

            var admin = new Admin
            {
                User = new User
                {
                    IdentityId = identityUser.Id,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    MobileNumber = mobile,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                }
            };

            context.Admins.Add(admin);
            await context.SaveChangesAsync(ct);
        }
    }

    private async Task CreateExpertAsync(string email, string firstName, string lastName, string mobile, CancellationToken ct)
    {
        if (await userManager.Users.AnyAsync(u => u.Email == email, ct))
            return;

        var identityUser = new IdentityUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(identityUser, "123456");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(identityUser, "Expert");

            var expert = new Expert
            {
                User = new User
                {
                    IdentityId = identityUser.Id,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    MobileNumber = mobile,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                }
            };

            context.Experts.Add(expert);
            await context.SaveChangesAsync(ct);
        }
    }

    private async Task CreateCustomerAsync(string email, string firstName, string lastName, string mobile, CancellationToken ct)
    {
        if (await userManager.Users.AnyAsync(u => u.Email == email, ct))
            return;

        var identityUser = new IdentityUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(identityUser, "123456");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(identityUser, "Customer");

            var cityId = await context.Cities.OrderBy(c => c.Id).Select(c => c.Id).FirstAsync(ct);

            var customer = new Customer
            {
                User = new User
                {
                    IdentityId = identityUser.Id,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    MobileNumber = mobile,
                    CityId = cityId,
                    IsActive = true,
                    Balance = 500000,
                    CreatedAt = DateTime.Now
                }
            };

            context.Customers.Add(customer);
            await context.SaveChangesAsync(ct);
        }
    }
}