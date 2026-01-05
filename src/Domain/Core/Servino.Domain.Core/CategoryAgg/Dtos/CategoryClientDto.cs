namespace Servino.Domain.Core.CategoryAgg.Dtos;

public class CategoryClientDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? ImagePath { get; set; }
    public bool HasChildren { get; set; }
}