using app.Domain.Common;

namespace app.Domain.Entities;

public class RequestImage :BaseEntity
{
    public int Id { get; set; }
    public string ImagePath { get; set; }
    public int RequestId { get; set; }
    public Request Request { get; set; }
}