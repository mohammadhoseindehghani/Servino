using Microsoft.EntityFrameworkCore;
using Servino.Domain.Core.UserAgg.Contracts.Data;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;

namespace Servino.Infa.DataAccess.Repo.EfCore.Repositories;

public class CustomerRepository(AppDbContext context) : ICustomerRepository
{
    public async Task<int> GetCustomerIdByUserIdAsync(int userId, CancellationToken ct)
    {
        return await context.Customers
            .Where(c => c.UserId == userId)
            .Select(c => c.Id)
            .FirstOrDefaultAsync(ct);
    }
}