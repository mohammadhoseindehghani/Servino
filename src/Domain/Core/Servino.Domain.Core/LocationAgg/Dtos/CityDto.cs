namespace Servino.Domain.Core.LocationAgg.Dtos;

public class CityDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int ProvinceId { get; set; }
    public string ProvinceName { get; set; }
}