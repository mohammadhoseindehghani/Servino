using app.Application.DTOs.Common;
using app.Domain.Enums;

namespace app.Application.DTOs.RequestDTOs;

public record RequestFullDto : BaseDto
{
    public string Title { get; init; }
    public string Description { get; init; }
    public string Address { get; init; }
    public int CityId { get; init; }
    public DateTime DateRequired { get; init; }
    public DateTime? DateDone { get; init; }
    public RequestStatus Status { get; init; }
    public int CustomerId { get; init; }
    public int CustomerUserId { get; init; }
    public int HomeServiceId { get; init; }
    public int? WinnerSuggestionId { get; init; }
    public List<string> ImagePaths { get; init; }
}