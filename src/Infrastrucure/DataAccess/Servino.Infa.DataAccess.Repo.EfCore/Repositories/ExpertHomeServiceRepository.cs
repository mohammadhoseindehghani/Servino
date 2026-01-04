using Microsoft.EntityFrameworkCore;
using Servino.Domain.Core.ExpertHomeServiceAgg.Contracts.Data;
using Servino.Domain.Core.ExpertHomeServiceAgg.Entity;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;

namespace Servino.Infa.DataAccess.Repo.EfCore.Repositories;

public class ExpertHomeServiceRepository(AppDbContext context) : IExpertHomeServiceRepository
{
    public async Task<List<int>> GetServiceIdsByExpertIdAsync(int expertId, CancellationToken ct)
    {
        return await context.ExpertHomeServices
            .AsNoTracking() 
            .Where(eh => eh.ExpertId == expertId)
            .Select(eh => eh.HomeServiceId)
            .ToListAsync(ct);
    }
    public async Task DeleteAllByExpertIdAsync(int expertId, CancellationToken ct)
    {
        await context.ExpertHomeServices
            .Where(eh => eh.ExpertId == expertId)
            .ExecuteDeleteAsync(ct);
    }
    public async Task AddRangeAsync(List<ExpertHomeService> expertServices, CancellationToken ct)
    {
        await context.ExpertHomeServices.AddRangeAsync(expertServices, ct);
        await context.SaveChangesAsync(ct);
    }
}