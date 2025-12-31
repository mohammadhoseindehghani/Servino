using Servino.Domain.Core._common;
using Servino.Domain.Core.CommentAgg.Entity;
using Servino.Domain.Core.HomeServiceAgg.Entity;
using Servino.Domain.Core.LocationAgg.Entity;
using Servino.Domain.Core.RequestAgg.Enum;
using Servino.Domain.Core.SuggestionAgg.Entity;
using Servino.Domain.Core.UserAgg.Entity;

namespace Servino.Domain.Core.RequestAgg.Entity;

public class Request : BaseEntity
{
    public string Title { get; set; } 
    public string Description { get; set; }
    public string Address { get; set; }
    public int CityId { get; set; }
    public City City { get; set; }
    public DateTime DateRequired { get; set; } 
    public DateTime? DateDone { get; set; } 
    public RequestStatus Status { get; set; }

    public int CustomerId { get; set; }
    public int HomeServiceId { get; set; }
    public int? CommentId { get; set; }
    public Comment Comment { get; set; }

    public int? WinnerSuggestionId { get; set; }

    public Customer Customer { get; set; }
    public HomeService HomeService { get; set; }
    public Suggestion? WinnerSuggestion { get; set; } 
    public ICollection<RequestImage> Images { get; set; }  
    public ICollection<Suggestion> Suggestions { get; set; }
}