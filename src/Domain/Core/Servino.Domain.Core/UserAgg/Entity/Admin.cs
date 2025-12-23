using Servino.Domain.Core._common;

namespace Servino.Domain.Core.UserAgg.Entity;

public class Admin : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
}