using Microsoft.EntityFrameworkCore;
using Servino.Domain.Core.UserAgg.Contracts.Data;
using Servino.Domain.Core.UserAgg.Dtos;
using Servino.Domain.Core.UserAgg.Entity;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;

namespace Servino.Infa.DataAccess.Repo.EfCore.Repositories;

public class CustomerRepository(AppDbContext context) : ICustomerRepository
{
    public async Task<bool> CreateAsync(int userId, CancellationToken ct)
    {
        var expert = new Expert { UserId = userId };
        context.Experts.Add(expert);
        return await context.SaveChangesAsync(ct) > 0;
    }
    public async Task<int> GetCustomerIdByUserIdAsync(int userId, CancellationToken ct)
    {
        return await context.Customers
            .Where(c => c.UserId == userId)
            .Select(c => c.Id)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<CustomerProfileDto?> GetByUserIdAsync(int userId, CancellationToken ct)
    {
        return await context.Customers.Where(c => c.UserId == userId)
            .Select(c => new CustomerProfileDto()
                {
                    UserId = c.UserId,
                    ExpertId = c.Id,
                    CityId = c.User.CityId,
                    FirstName = c.User.FirstName,
                    LastName = c.User.LastName,
                    Balance = c.User.Balance,
                    Email = c.User.Email,
                    Phone = c.User.MobileNumber,
                }
            ).FirstOrDefaultAsync(ct);
    }

    public async Task<bool> UpdateProfile(UpdateCustomerProfileDto command, CancellationToken ct)
    {
        var customer = await context.Customers.Include(c => c.User)
            .FirstOrDefaultAsync(c => c.UserId == command.UserId, ct);

        if (customer == null) return false;

        customer.User.FirstName = command.FirstName ?? customer.User.FirstName;
        customer.User.LastName = command.LastName ?? customer.User.LastName;
        customer.User.CityId = command.CityId ?? customer.User.CityId;
        customer.User.ProfileImagePath = command.ProfileImagePath ?? customer.User.ProfileImagePath;
        await context.SaveChangesAsync(ct);
        return true;
    }
}