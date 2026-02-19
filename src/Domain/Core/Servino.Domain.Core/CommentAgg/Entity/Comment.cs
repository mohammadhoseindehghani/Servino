using Servino.Domain.Core._common;
using Servino.Domain.Core.CommentAgg.Enum;
using Servino.Domain.Core.RequestAgg.Entity;
using Servino.Domain.Core.UserAgg.Entity;

namespace Servino.Domain.Core.CommentAgg.Entity;

public class Comment : BaseEntity
{
    public string Title { get; set; }
    public string Text { get; set; }
    public Rate Rating { get; set; } 

    public bool IsApproved { get; set; }

    public int CustomerId { get; set; }
    public int ExpertId { get; set; }
    public int RequestId { get; set; }

    public Customer Customer { get; set; }
    public Expert Expert { get; set; } 
    public Request Request { get; set; }
}