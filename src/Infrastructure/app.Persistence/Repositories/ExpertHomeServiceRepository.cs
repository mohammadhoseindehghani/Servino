using app.Application.Contracts.Repositories;
using app.Application.DTOs.UserDTOs;
using app.Domain.ExpertHomeServiceAgg.Entities;
using Microsoft.EntityFrameworkCore;

namespace app.Persistence.Repositories;

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
    public async Task<List<ExpertServiceItemDto>> GetExpertServicesByExpertIdAsync(int expertId, CancellationToken ct)
    {
        return await context.ExpertHomeServices.Where(e => e.ExpertId == expertId)
            .Select(e => new ExpertServiceItemDto
            {
                HomeServiceId = e.Id,
                HomeServiceTitle = e.HomeService.Title,
                IsSelected = true
            }).ToListAsync(ct);
    }
}