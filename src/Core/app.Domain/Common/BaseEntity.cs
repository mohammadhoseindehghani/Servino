namespace app.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; protected set; }

    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? LastModifiedAt { get; protected set; }

    public void MarkAsModified()
    {
        LastModifiedAt = DateTime.UtcNow;
    }
}