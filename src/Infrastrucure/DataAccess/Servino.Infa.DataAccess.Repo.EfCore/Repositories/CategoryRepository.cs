using Microsoft.EntityFrameworkCore;
using Servino.Domain.Core._common;
using Servino.Domain.Core.CategoryAgg.Contracts.Data;
using Servino.Domain.Core.CategoryAgg.Dtos;
using Servino.Domain.Core.CategoryAgg.Entity;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;

namespace Servino.Infa.DataAccess.Repo.EfCore.Repositories;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    public async Task<bool> CreateAsync(CategoryDto command, CancellationToken ct)
    {
        var category = new Category
        {
            Title = command.Title,
            ImagePath = command.ImagePath,
            ParentId = command.ParentId,
            IsActive = true
        };

        context.Categories.Add(category);
        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateAsync(CategoryDto command, CancellationToken ct)
    {
        var affectedRows = await context.Categories
            .Where(c => c.Id == command.Id && !c.IsDeleted)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(c => c.Title, command.Title)
                    .SetProperty(c => c.ParentId, command.ParentId)
                    .SetProperty(c => c.ImagePath, c => command.ImagePath ?? c.ImagePath)
                    .SetProperty(c => c.UpdatedAt, DateTime.Now),
                ct);

        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (category == null) return false;

        category.IsDeleted = true; 
        category.DeletedAt = DateTime.Now;

        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await context.Categories
            .AsNoTracking() 
            .Where(c => c.Id == id)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Title = c.Title,
                ParentId = c.ParentId,
                ImagePath = c.ImagePath
            }).FirstOrDefaultAsync(ct);
    }

    public async Task<List<CategorySummaryDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        var query = context.Categories.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search.SearchKey))
        {
            query = query.Where(c => c.Title.Contains(search.SearchKey));
        }

        return await query
            .OrderByDescending(c => c.CreatedAt) 
            .Skip((search.PageNumber - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(c => new CategorySummaryDto
            {
                Id = c.Id,
                Title = c.Title,
                ParentTitle = c.Parent != null ? c.Parent.Title : "-", 
                SubCategoriesCount = c.SubCategories.Count, 
                IsActive = c.IsActive
            })
            .ToListAsync(ct);
    }

    public async Task<int> GetCountAsync(CancellationToken ct)
    {
        return await context.Categories.CountAsync(ct);
    }
}