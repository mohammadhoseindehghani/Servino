using app.Application.Contracts.Repositories;
using app.Application.DTOs.UserDTOs;
using app.Domain.UserAgg.Entities;
using Microsoft.EntityFrameworkCore;

namespace app.Persistence.Repositories;

public class CustomerRepository(AppDbContext context) : ICustomerRepository
{
    public async Task<bool> CreateAsync(int userId, CancellationToken ct)
    {
        var customer = new Customer() { UserId = userId };
        context.Customers.Add(customer);
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
                    ProfileImagePath = c.User.ProfileImagePath
                }
            ).FirstOrDefaultAsync(ct);
    }

    public async Task<bool> UpdateProfile(UpdateCustomerProfileDto dto, CancellationToken ct)
    {
        var affectedRows = await context.Users
            .Where(u => u.Id == dto.UserId)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.FirstName, u => dto.FirstName ?? u.FirstName)
                    .SetProperty(u => u.LastName, u => dto.LastName ?? u.LastName)
                    .SetProperty(u => u.CityId, u => dto.CityId ?? u.CityId)
                    .SetProperty(u => u.ProfileImagePath,
                        u => dto.ProfileImagePath ?? u.ProfileImagePath)
                    .SetProperty(u => u.UpdatedAt, DateTime.UtcNow),
                ct);
        return affectedRows > 0;
    }

    public async Task<bool> HardDeleteByUserIdAsync(int userId, CancellationToken ct)
    {
        var effectiveRows = await context.Customers.Where(e => e.UserId == userId)
            .ExecuteDeleteAsync(ct);

        return effectiveRows > 0;
    }
}