using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Servino.Domain.Core._common.Base;
using Servino.Domain.Core.CategoryAgg.Entity;
using Servino.Domain.Core.ExpertHomeServiceAgg.Entity;
using Servino.Domain.Core.HomeServiceAgg.Entity;
using Servino.Domain.Core.LocationAgg.Entity;
using Servino.Domain.Core.RequestAgg.Enum;
using Servino.Domain.Core.SuggestionAgg.Enum;
using Servino.Domain.Core.UserAgg.Entity;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;

namespace Servino.Infa.Db.SqlServer.EfCore.DataSeed;

public class DbInitializer(
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    AppDbContext context)
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly AppDbContext _context = context;

    public void Seed()
    {
        _context.Database.Migrate();

        SeedEnums();
        SeedRoles();
        SeedLocations();
        SeedServices();
        SeedUsers();
    }

    private void SeedEnums()
    {
        if (!_context.RequestStatuses.Any())
        {
            foreach (var item in Enum.GetValues(typeof(RequestStatus)))
            {
                _context.RequestStatuses.Add(new RequestStatusLookup { Id = (int)item, Title = item.ToString() });
            }
        }
        if (!_context.SuggestionStatuses.Any())
        {
            foreach (var item in Enum.GetValues(typeof(SuggestionStatus)))
            {
                _context.SuggestionStatuses.Add(new SuggestionStatusLookup { Id = (int)item, Title = item.ToString() });
            }
        }
        _context.SaveChanges();
    }

    private void SeedRoles()
    {
        if (!_roleManager.RoleExistsAsync("Admin").Result) _roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
        if (!_roleManager.RoleExistsAsync("Expert").Result) _roleManager.CreateAsync(new IdentityRole("Expert")).Wait();
        if (!_roleManager.RoleExistsAsync("Customer").Result) _roleManager.CreateAsync(new IdentityRole("Customer")).Wait();
    }

    private void SeedLocations()
    {
        if (!_context.Provinces.Any())
        {
            foreach (var provinceData in IranLocationData.Provinces)
            {
                var province = new Province
                {
                    Title = provinceData.Key
  
                };

                foreach (var cityTitle in provinceData.Value)
                {
                    province.Cities.Add(new City { Title = cityTitle });
                }
                _context.Provinces.Add(province);
            }

            _context.SaveChanges();
        }
    }

    private void SeedServices()
    {
        if (!_context.Categories.Any())
        {
            var allCategories = ServiceData.GetCategories();
            _context.Categories.AddRange(allCategories);
            _context.SaveChanges();
        }
    }

    private void SeedUsers()
    {
        if (!_userManager.Users.Any(u => u.Email == "admin@servino.com"))
        {
            var user = new IdentityUser { UserName = "admin@servino.com", Email = "admin@servino.com", EmailConfirmed = true };
            var result = _userManager.CreateAsync(user, "123456").Result;
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, "Admin").Wait();
                _context.Admins.Add(new Admin
                {
                    User = new User
                    {
                        IdentityId = user.Id,
                        FirstName = "مدیر",
                        LastName = "سیستم",
                        Email = "admin@servino.com",
                        MobileNumber = "09120000000",
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    }
                });
            }
        }

        if (!_userManager.Users.Any(u => u.Email == "expert@servino.com"))
        {
            var user = new IdentityUser { UserName = "expert@servino.com", Email = "expert@servino.com", EmailConfirmed = true };
            var result = _userManager.CreateAsync(user, "123456").Result;
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, "expert").Wait();
                _context.Admins.Add(new Admin
                {
                    User = new User
                    {
                        IdentityId = user.Id,
                        FirstName = "اکسپرت",
                        LastName = "سیستم",
                        Email = "expert@servino.com",
                        MobileNumber = "09130000000",
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    }
                });
            }
        }

        if (!_userManager.Users.Any(u => u.Email == "customer@servino.com"))
        {
            var user = new IdentityUser { UserName = "customer@servino.com", Email = "customer@servino.com", EmailConfirmed = true };
            var result = _userManager.CreateAsync(user, "123456").Result;
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, "Customer").Wait();
                var cityId = _context.Cities.First().Id;
                _context.Customers.Add(new Customer
                {
                    User = new User
                    {
                        IdentityId = user.Id,
                        FirstName = "علی",
                        LastName = "مشتری",
                        Email = "customer@servino.com",
                        MobileNumber = "09121111111",
                        CityId = cityId,
                        IsActive = true,
                        Balance = 500000,
                        CreatedAt = DateTime.Now
                    }
                });
            }
        }
        _context.SaveChanges();
    }
}