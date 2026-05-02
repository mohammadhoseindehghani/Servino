using app.Domain._common;

namespace app.Domain.UserAgg.Entities;

public class Admin : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
}