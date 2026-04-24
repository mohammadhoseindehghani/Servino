using app.Domain.Common;

namespace app.Domain.Entities;

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