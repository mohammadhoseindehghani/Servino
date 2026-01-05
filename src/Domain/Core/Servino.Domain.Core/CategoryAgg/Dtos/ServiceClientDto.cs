namespace Servino.Domain.Core.CategoryAgg.Dtos;

public class ServiceClientDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? ShortDescription { get; set; }
    public decimal BasePrice { get; set; }
    public string? ImagePath { get; set; }
}