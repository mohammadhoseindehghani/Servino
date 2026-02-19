using Servino.Domain.Core._common;

namespace Servino.Domain.Core.LocationAgg.Entity;

public class Province : BaseEntity
{
    public string Title { get; set; }
    public ICollection<City> Cities { get; set; }

    public Province()
    {
        Cities = new List<City>();
    }
}