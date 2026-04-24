using app.Application.DTOs.Common;
using app.Domain.Enums;

namespace app.Application.DTOs.RequestDTOs;

public record UpdateRequestDto : BaseDto
{
    public string Title { get; init; }
    public string Description { get; init; }
    public string Address { get; init; }
    public int CityId { get; init; }
    public DateTime DateRequired { get; init; }
    public RequestStatus Status { get; init; }
    public int? WinnerSuggestionId { get; init; }
    public DateTime? DateDone { get; init; }
}