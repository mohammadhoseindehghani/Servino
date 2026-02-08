using Servino.Domain.Core.CommentAgg.Enum;

namespace Servino.Domain.Core.CommentAgg.Dtos;

public class CommentDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public Rate Rating { get; set; }
    public bool IsApproved { get; set; }
    public DateTime CreatedAt { get; set; } 
    public string CustomerName { get; set; }
    public string ExpertName { get; set; }
    prop
}