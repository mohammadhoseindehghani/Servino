using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.DTOs.LocationDTOs;
using app.Domain.LocationAgg.Entities;
using Microsoft.EntityFrameworkCore;

namespace app.Persistence.Repositories;

public class CityRepository(AppDbContext context) : ICityRepository
{
    public async Task<bool> CreateAsync(string title, int provinceId, CancellationToken ct)
    {
        var city = new City { Title = title, ProvinceId = provinceId };
        context.Cities.Add(city);
        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(int id, string title, int provinceId, CancellationToken ct)
    {
        var affectedRows = await context.Cities
            .Where(c => c.Id == id && !c.IsDeleted)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(c => c.Title, title)
                    .SetProperty(c => c.ProvinceId, provinceId)
                    .SetProperty(c => c.UpdatedAt, DateTime.UtcNow),
                ct);

        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var affectedRows = await context.Cities
            .Where(c => c.Id == id && !c.IsDeleted)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(c => c.IsDeleted, true)
                    .SetProperty(c => c.DeletedAt, DateTime.UtcNow),
                ct);

        return affectedRows > 0;
    }

    public async Task<CityDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await context.Cities
            .AsNoTracking()
            .Where(c => c.Id == id && !c.IsDeleted)
            .Select(c => new CityDto
            {
                Id = c.Id,
                Title = c.Title,
                ProvinceId = c.ProvinceId,
                ProvinceName = c.Province.Title 
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<CityDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        var query = context.Cities.AsNoTracking()
            .Include(c => c.Province) 
            .Where(c => !c.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search.SearchKey))
        {
            query = query.Where(c => c.Title.Contains(search.SearchKey) || c.Province.Title.Contains(search.SearchKey));
        }

        return await query
            .OrderByDescending(c => c.Id)
            .Skip((search.PageNumber - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(c => new CityDto
            {
                Id = c.Id,
                Title = c.Title,
                ProvinceId = c.ProvinceId,
                ProvinceName = c.Province.Title
            })
            .ToListAsync(ct);
    }

    public async Task<int> GetCountAsync(CancellationToken ct)
    {
        return await context.Cities.CountAsync(c => !c.IsDeleted, ct);
    }

    public async Task<List<SelectListDto>> GetCitiesByProvinceIdAsync(int provinceId, CancellationToken ct)
    {
        return await context.Cities
            .AsNoTracking()
            .Where(c => c.ProvinceId == provinceId && !c.IsDeleted)
            .OrderBy(c => c.Title) 
            .Select(c => new SelectListDto { Id = c.Id, Title = c.Title })
            .ToListAsync(ct);
    }
}