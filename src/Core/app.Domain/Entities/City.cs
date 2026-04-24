using app.Domain.Common;

namespace app.Domain.Entities;

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