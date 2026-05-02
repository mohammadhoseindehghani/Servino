using app.Domain._common;

namespace app.Domain.RequestAgg.Entities;

public class RequestImage :BaseEntity
{
    public int Id { get; set; }
    public string ImagePath { get; set; }
    public int RequestId { get; set; }
    public Request Request { get; set; }
}