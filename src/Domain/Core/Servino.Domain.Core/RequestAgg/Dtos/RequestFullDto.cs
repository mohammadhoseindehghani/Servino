using Servino.Domain.Core.RequestAgg.Enum;

namespace Servino.Domain.Core.RequestAgg.Dtos;

public class RequestFullDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public int CityId { get; set; }
    public DateTime DateRequired { get; set; }
    public DateTime? DateDone { get; set; }
    public RequestStatus Status { get; set; }
    public int CustomerId { get; set; }
    public int CustomerUserId { get; set; }
    public int HomeServiceId { get; set; }
    public int? WinnerSuggestionId { get; set; }
    public List<string> ImagePaths { get; set; }
}