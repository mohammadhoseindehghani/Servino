using app.Domain.Common;

namespace app.Domain.Entities;

public class Customer : BaseEntity
{
    public int UserId { get; set; }

    public User User { get; set; }
    public ICollection<Request> Requests { get; set; } 
    public ICollection<Comment> Comments { get; set; } 
}