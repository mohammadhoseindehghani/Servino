using app.Domain._common;
using app.Domain.HomeServiceAgg.Entities;

namespace app.Domain.CategoryAgg.Entities;

public class Category : BaseEntity
{
    public string Title { get; set; }
    public string? ImagePath { get; set; }
    public bool IsActive { get; set; }

    public int? ParentId { get; set; }

    public Category? Parent { get; set; }
    public List<Category> SubCategories { get; set; } 
    public List<HomeService> Services { get; set; }

    private Category()
    {
    }

    public Category(string title, string? imagePath, int? parentId)
    {
        Title = title;
        ImagePath = imagePath;
        ParentId = parentId;
    }
}