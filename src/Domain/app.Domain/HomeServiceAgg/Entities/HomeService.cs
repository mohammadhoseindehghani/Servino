using app.Domain._common;
using app.Domain.CategoryAgg.Entities;
using app.Domain.ExpertHomeServiceAgg.Entities;
using app.Domain.RequestAgg.Entities;

namespace app.Domain.HomeServiceAgg.Entities;

public class HomeService : BaseEntity
{
    public string Title { get; set; }
    public string? ShortDescription { get; set; }
    public decimal BasePrice { get; set; }
    public string? ImagePath { get; set; }
    public bool IsActive { get; set; }
    public int VisitCount { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; }
    public ICollection<ExpertHomeService> ExpertHomeServices { get; set; }
    public ICollection<Request> Requests { get; set; }

    public HomeService()
    {
        Requests = new List<Request>();
        ExpertHomeServices = new List<ExpertHomeService>();
    }

}