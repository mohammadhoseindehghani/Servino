using app.Domain.Common;

namespace app.Domain.Entities;

public class Category : BaseEntity
{
    public string Title { get; set; }
    public string? ImagePath { get; set; }
    public bool IsActive { get; set; }

    public int? ParentId { get; set; }

    public Category? Parent { get; set; }
    public ICollection<Category> SubCategories { get; set; } 
    public ICollection<HomeService> Services { get; set; }

    public Category()
    {
        SubCategories = new List<Category>();
        Services = new List<HomeService>();
    }
}