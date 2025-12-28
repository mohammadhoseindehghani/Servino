using Servino.Domain.Core.CommentAgg.Enum;

namespace Servino.Domain.Core.CommentAgg.Dtos;

public class CreateCommentDto
{
    public string Title { get; set; }
    public string Text { get; set; }
    public Rate Rating { get; set; }
    public int CustomerId { get; set; }
    public int ExpertId { get; set; }
    public int RequestId { get; set; }
}