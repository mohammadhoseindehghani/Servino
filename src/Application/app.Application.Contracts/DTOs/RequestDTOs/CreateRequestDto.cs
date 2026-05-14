namespace app.Application.Contracts.DTOs.RequestDTOs;

public record CreateRequestDto
{
    public string Title { get; init; }
    public string Description { get; init; }
    public string Address { get; init; }
    public int CityId { get; init; }
    public DateTime DateRequired { get; init; }
    public int CustomerId { get; init; }
    public int HomeServiceId { get; init; }
    public List<string>? ImagePaths { get; init; }
}