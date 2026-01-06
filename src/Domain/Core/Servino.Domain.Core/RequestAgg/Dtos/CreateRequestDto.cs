namespace Servino.Domain.Core.RequestAgg.Dtos;

public class CreateRequestDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public int CityId { get; set; }
    public DateTime DateRequired { get; set; }
    public int CustomerId { get; set; }
    public int HomeServiceId { get; set; }
    public List<string>? ImagePaths { get; set; }
}