using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Servino.Domain.Core._common;
using Servino.Domain.Core.HomeServiceAgg.Contracts.Data;
using Servino.Domain.Core.HomeServiceAgg.Dtos;
using Servino.Domain.Core.HomeServiceAgg.Entity;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;

namespace Servino.Infa.DataAccess.Repo.EfCore.Repositories;

public class HomeServiceRepository(AppDbContext context, ILogger<HomeServiceRepository> logger) : IHomeServiceRepository
{
    public async Task<List<HomeServiceSummaryDto>> GetAllActiveServicesAsync(CancellationToken ct)
    {
        logger.LogInformation($"[Repo] GetAllActiveServices started");

        var result = await context.HomeServices
            .Where(hs => hs.IsActive && !hs.IsDeleted)
            .Select(hs => new HomeServiceSummaryDto
            {
                Id = hs.Id,
                Title = hs.Title,
                CategoryName = hs.Category.Title,
                BasePrice = hs.BasePrice.ToString("N0"),
                VisitCount = hs.VisitCount,
                ImagePath = hs.ImagePath
            })
            .ToListAsync(ct);

        logger.LogInformation($"[Repo] GetAllActiveServices finished. Count={result.Count}");

        return result;
    }

    public async Task<bool> CreateAsync(HomeServiceDto command, CancellationToken ct)
    {
        var service = new HomeService
        {
            Title = command.Title,
            BasePrice = command.BasePrice,
            CategoryId = command.CategoryId,
            ShortDescription = command.ShortDescription,
            ImagePath = command.ImagePath, 
            IsActive = true,
            VisitCount = 0
        };
        context.HomeServices.Add(service);
        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(HomeServiceDto command, CancellationToken ct)
    {

        var affectedRows = await context.HomeServices
            .Where(hs => hs.Id == command.Id && !hs.IsDeleted)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(hs => hs.Title, command.Title)
                .SetProperty(hs => hs.BasePrice, command.BasePrice)
                .SetProperty(hs => hs.CategoryId, command.CategoryId)
                .SetProperty(hs => hs.ShortDescription, command.ShortDescription)
                .SetProperty(hs => hs.ImagePath, command.ImagePath) 
                                                                 
                .SetProperty(hs => hs.UpdatedAt, DateTime.Now),
                ct);

        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var affectedRows = await context.HomeServices
            .Where(hs => hs.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(hs => hs.IsDeleted, true)
                .SetProperty(hs => hs.DeletedAt, DateTime.Now), 
                ct);

        return affectedRows > 0;
    }

    public async Task<HomeServiceDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await context.HomeServices
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new HomeServiceDto
            {
                Id = s.Id,
                Title = s.Title,
                BasePrice = s.BasePrice,
                CategoryId = s.CategoryId,
                ShortDescription = s.ShortDescription,
                ImagePath = s.ImagePath 
            }).FirstOrDefaultAsync(ct);
    }

    public async Task<List<HomeServiceSummaryDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        var query = context.HomeServices.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search.SearchKey))
        {
            query = query.Where(s => s.Title.Contains(search.SearchKey));
        }

        return await query
            .OrderByDescending(s => s.VisitCount)
            .Skip((search.PageNumber - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(s => new HomeServiceSummaryDto
            {
                Id = s.Id,
                Title = s.Title,
                CategoryName = s.Category.Title,
                BasePrice = s.BasePrice.ToString("N0"),
                VisitCount = s.VisitCount,
                ImagePath = s.ImagePath 
            })
            .ToListAsync(ct);
    }
}