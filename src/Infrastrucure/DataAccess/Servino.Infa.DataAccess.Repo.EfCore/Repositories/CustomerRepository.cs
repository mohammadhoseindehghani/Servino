using Microsoft.EntityFrameworkCore;
using Servino.Domain.Core.UserAgg.Contracts.Data;
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
}