using app.Domain.CommentAgg.Entities;
using app.Domain.Common;
using app.Domain.RequestAgg.Entities;

namespace app.Domain.UserAgg.Entities;

public class Customer : BaseEntity
{
    public int UserId { get; set; }

    public User User { get; set; }
    public ICollection<Request> Requests { get; set; } 
    public ICollection<Comment> Comments { get; set; } 
}