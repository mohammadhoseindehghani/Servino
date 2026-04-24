using app.Domain.Common;

namespace app.Domain.Entities;

public class Admin : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
}