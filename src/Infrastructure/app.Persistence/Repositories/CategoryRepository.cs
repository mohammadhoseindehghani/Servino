using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.DTOs.CategoryDTOs;
using app.Domain.CategoryAgg.Entities;
using Microsoft.EntityFrameworkCore;

namespace app.Persistence.Repositories;

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
        var affectedRows = await context.Categories
            .Where(c => c.Id == id && !c.IsDeleted)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(c => c.IsDeleted, true)
                    .SetProperty(c => c.DeletedAt, DateTime.UtcNow),
                ct);

        return affectedRows > 0;
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
                ParentTitle = c.Parent!.Title,
                SubCategoriesCount = c.SubCategories.Count,
                IsActive = c.IsActive,
                ImagePath = c.ImagePath
            })
            .ToListAsync(ct);
    }

    public async Task<int> GetCountAsync(CancellationToken ct)
    {
        return await context.Categories.CountAsync(ct);
    }

    public async Task<List<CategoryClientDto>> GetCategoriesByParentIdAsync(int? parentId, CancellationToken ct)
    {
        var query = context.Categories.AsNoTracking().AsQueryable();
        query = query.Where(c => c.ParentId == parentId);
        query = query.Where(c => !c.IsDeleted && c.IsActive);
        return await query.Select(c => new CategoryClientDto
        {
            Id = c.Id,
            Title = c.Title,
            ImagePath = c.ImagePath,
            HasChildren = c.SubCategories.Any(sc => !sc.IsDeleted && sc.IsActive)
        }).ToListAsync(ct);
    }

    public async Task<List<ServiceClientDto>> GetServicesByCategoryIdAsync(int categoryId, CancellationToken ct)
    {
        return await context.HomeServices
            .AsNoTracking()
            .Where(s => s.CategoryId == categoryId && !s.IsDeleted)
            .Select(s => new ServiceClientDto
            {
                Id = s.Id,
                Title = s.Title,
                BasePrice = s.BasePrice,
                ShortDescription = s.ShortDescription,
                ImagePath = s.ImagePath
            })
            .ToListAsync(ct);
    }

    public async Task<List<BreadcrumbDto>> GetBreadcrumbAsync(int categoryId, CancellationToken ct)
    {
        FormattableString sql = $@"
    WITH RecursiveCategory AS (
        SELECT Id, Title, ParentId, 1 as Level
        FROM Categories
        WHERE Id = {categoryId} AND IsDeleted = 0 
        
        UNION ALL
        
        SELECT c.Id, c.Title, c.ParentId, rc.Level + 1
        FROM Categories c
        INNER JOIN RecursiveCategory rc ON c.Id = rc.ParentId
        WHERE c.IsDeleted = 0 
    )
    SELECT Id, Title, ParentId, Level 
    FROM RecursiveCategory 
    ORDER BY Level DESC";

        return await context.Database
            .SqlQuery<BreadcrumbDto>(sql)
            .ToListAsync(ct);
    }

    public async Task<bool> IsCategoryExistAndActiveAsync(int id, CancellationToken ct)
    {
        return await context.Categories
            .AnyAsync(c => c.Id == id && !c.IsDeleted && c.IsActive, ct);
    }
}
