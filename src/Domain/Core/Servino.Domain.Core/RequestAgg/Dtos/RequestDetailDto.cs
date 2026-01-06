using Servino.Domain.Core.RequestAgg.Enum;

namespace Servino.Domain.Core.RequestAgg.Dtos;

public class RequestDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public string CityName { get; set; }
    public DateTime DateRequired { get; set; }
    public RequestStatus RequestStatus { get; set; }
    public List<string> ImagePaths { get; set; }
    public string CustomerName { get; set; }
}