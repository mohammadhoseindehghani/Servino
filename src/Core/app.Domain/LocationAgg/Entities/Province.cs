using app.Domain.Common;

namespace app.Domain.LocationAgg.Entities;

public class Province : BaseEntity
{
    public string Title { get; set; }
    public ICollection<City> Cities { get; set; }

    public Province()
    {
        Cities = new List<City>();
    }
}