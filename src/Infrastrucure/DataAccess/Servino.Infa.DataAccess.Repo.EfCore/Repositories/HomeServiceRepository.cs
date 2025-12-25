using Microsoft.EntityFrameworkCore;
using Servino.Domain.Core._common;
using Servino.Domain.Core.HomeServiceAgg.Contracts.Data;
using Servino.Domain.Core.HomeServiceAgg.Dtos;
using Servino.Domain.Core.HomeServiceAgg.Entity;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;

namespace Servino.Infa.DataAccess.Repo.EfCore.Repositories;

public class HomeServiceRepository(AppDbContext context) : IHomeServiceRepository
{
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
            .Where(x => x.Id == command.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.Title, command.Title)
                .SetProperty(x => x.BasePrice, command.BasePrice)
                .SetProperty(x => x.CategoryId, command.CategoryId)
                .SetProperty(x => x.ShortDescription, command.ShortDescription)
                .SetProperty(x => x.ImagePath, command.ImagePath) 
                                                                 
                .SetProperty(x => x.UpdatedAt, DateTime.Now),
                ct);

        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var affectedRows = await context.HomeServices
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.IsDeleted, true)
                .SetProperty(x => x.DeletedAt, DateTime.Now), 
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