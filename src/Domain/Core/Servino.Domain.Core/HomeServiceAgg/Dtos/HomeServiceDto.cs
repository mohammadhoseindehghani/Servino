namespace Servino.Domain.Core.HomeServiceAgg.Dtos;

public class HomeServiceDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal BasePrice { get; set; }
    public int CategoryId { get; set; }
    public string? ShortDescription { get; set; }
    public string? ImagePath { get; set; }
}