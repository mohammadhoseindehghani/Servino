using app.Domain.CommentAgg.Entities;
using app.Domain.Common;
using app.Domain.HomeServiceAgg.Entities;
using app.Domain.LocationAgg.Entities;
using app.Domain.SuggestionAgg.Entities;
using app.Domain.SuggestionAgg.Enums;
using app.Domain.UserAgg.Entities;

namespace app.Domain.RequestAgg.Entities;

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
    public DateTime? NoSuggestionReminderAt { get; set; }

    public int CustomerId { get; set; }
    public int HomeServiceId { get; set; }
    public int? CommentId { get; set; }
    public Comment Comment { get; set; }

    public int? WinnerSuggestionId { get; set; }

    public Customer Customer { get; set; }
    public HomeService HomeService { get; set; }
    public Suggestion? WinnerSuggestion { get; set; } 
    public ICollection<RequestImage>? Images { get; set; }  
    public ICollection<Suggestion> Suggestions { get; set; }
}