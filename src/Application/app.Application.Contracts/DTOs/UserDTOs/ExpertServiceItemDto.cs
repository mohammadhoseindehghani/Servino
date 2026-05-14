namespace app.Application.Contracts.DTOs.UserDTOs;

public record ExpertServiceItemDto
{
    public int HomeServiceId { get; init; }
    public string HomeServiceTitle { get; init; }
    public bool IsSelected { get; init; }
}