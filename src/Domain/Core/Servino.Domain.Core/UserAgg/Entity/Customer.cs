using Servino.Domain.Core._common;
using Servino.Domain.Core.CommentAgg.Entity;
using Servino.Domain.Core.RequestAgg.Entity;

namespace Servino.Domain.Core.UserAgg.Entity;

public class Customer : BaseEntity
{
    public int UserId { get; set; }

    public User User { get; set; }
    public ICollection<Request> Requests { get; set; } 
    public ICollection<Comment> Comments { get; set; } 
}