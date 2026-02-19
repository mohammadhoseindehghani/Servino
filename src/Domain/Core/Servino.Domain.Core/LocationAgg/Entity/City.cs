using Servino.Domain.Core._common;
using Servino.Domain.Core.RequestAgg.Entity;
using Servino.Domain.Core.UserAgg.Entity;

namespace Servino.Domain.Core.LocationAgg.Entity;

public class City : BaseEntity
{
    public string Title { get; set; }

    public int ProvinceId { get; set; }
    public Province Province { get; set; }
    public ICollection<User> Users { get; set; }
    public ICollection<Request> Requests { get; set; }

    public City()
    {
        Users = new List<User>();
        Requests = new List<Request>();
    }
}