using Servino.Domain.Core._common;

namespace Servino.Domain.Core.RequestAgg.Entity;

public class RequestImage :BaseEntity
{
    public int Id { get; set; }
    public string ImagePath { get; set; }
    public int RequestId { get; set; }
    public Request Request { get; set; }
}