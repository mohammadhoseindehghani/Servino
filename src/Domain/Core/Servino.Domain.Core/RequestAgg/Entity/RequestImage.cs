namespace Servino.Domain.Core.RequestAgg.Entity;

public class RequestImage 
{
    public string ImagePath { get; set; }
    public int RequestId { get; set; }
    public Request Request { get; set; }
}