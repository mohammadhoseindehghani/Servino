using app.Application.DTOs.Common;
using app.Domain.Enums;

namespace app.Application.DTOs.RequestDTOs;

public record RequestDetailDto : BaseDto
{
    public string Title { get; init; }
    public string Description { get; init; }
    public string Address { get; init; }
    public string CityName { get; init; }
    public DateTime DateRequired { get; init; }
    public RequestStatus RequestStatus { get; init; }
    public List<string> ImagePaths { get; init; }
    public string CustomerName { get; init; }
    public int CustomerId { get; init; }
}