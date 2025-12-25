namespace Servino.Domain.Core.HomeServiceAgg.Dtos;

public class HomeServiceSummaryDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string CategoryName { get; set; } 
    public string BasePrice { get; set; } 
    public int VisitCount { get; set; }
    public string? ImagePath { get; set; }
}