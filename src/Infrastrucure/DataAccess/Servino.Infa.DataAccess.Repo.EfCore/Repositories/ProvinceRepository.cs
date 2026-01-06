using Microsoft.EntityFrameworkCore;
using Servino.Domain.Core._common;
using Servino.Domain.Core.LocationAgg.Contracts.Data;
using Servino.Domain.Core.LocationAgg.Dtos;
using Servino.Domain.Core.LocationAgg.Entity;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;

namespace Servino.Infa.DataAccess.Repo.EfCore.Repositories;

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
        var province = await context.Provinces.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (province == null) return false;

        province.Title = title;
        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var province = await context.Provinces.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (province == null) return false;

        province.IsDeleted = true;
        province.DeletedAt = DateTime.Now;
        return await context.SaveChangesAsync(ct) > 0;
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