
namespace app.Domain._common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? CreatedBy { get; set; }

    public DateTime? ModifiesAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public int? DeletedBy { get; set; }
}

