namespace Servino.Domain.Core.CategoryAgg.Dtos;

public class BreadcrumbDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int? ParentId { get; set; }
}