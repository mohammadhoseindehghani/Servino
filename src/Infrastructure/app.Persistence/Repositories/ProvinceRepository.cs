using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.DTOs.LocationDTOs;
using app.Domain.LocationAgg.Entities;
using Microsoft.EntityFrameworkCore;

namespace app.Persistence.Repositories;

public class ProvinceRepository(AppDbContext context) : IProvinceRepository
{
    public async Task<bool> CreateAsync(string title, CancellationToken ct)
    {
        var province = new Province { Title = title }; 
        context.Provinces.Add(province);
        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(int id, string title, CancellationToken ct)
    {
        var affectedRows = await context.Provinces
            .Where(p => p.Id == id && !p.IsDeleted)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.Title, title)
                    .SetProperty(p => p.UpdatedAt, DateTime.UtcNow),
                ct);

        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var affectedRows = await context.Provinces
            .Where(p => p.Id == id && !p.IsDeleted)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.IsDeleted, true)
                    .SetProperty(p => p.DeletedAt, DateTime.UtcNow),
                ct);

        return affectedRows > 0;
    }

    public async Task<ProvinceDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await context.Provinces
            .AsNoTracking()
            .Where(p => p.Id == id && !p.IsDeleted)
            .Select(p => new ProvinceDto { Id = p.Id, Title = p.Title })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<ProvinceDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        var query = context.Provinces.AsNoTracking().Where(p => !p.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search.SearchKey))
        {
            query = query.Where(p => p.Title.Contains(search.SearchKey));
        }

        return await query
            .OrderByDescending(p => p.Id)
            .Skip((search.PageNumber - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(p => new ProvinceDto
            {
                Id = p.Id,
                Title = p.Title,
                CityCount = p.Cities.Count(c => !c.IsDeleted)
            })
            .ToListAsync(ct);
    }

    public async Task<int> GetCountAsync(CancellationToken ct)
    {
        return await context.Provinces.CountAsync(p => !p.IsDeleted, ct);
    }

    public async Task<List<SelectListDto>> GetAllForDropdownAsync(CancellationToken ct)
    {
        return await context.Provinces
            .AsNoTracking()
            .Where(p => !p.IsDeleted)
            .Select(p => new SelectListDto { Id = p.Id, Title = p.Title })
            .ToListAsync(ct);
    }
}