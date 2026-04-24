using app.Domain.Enums;

namespace app.Application.DTOs.CommentDTOs;

public record CreateCommentDto
{
    public string Title { get; init; }
    public string Text { get; init; }
    public Rate Rating { get; init; }
    public int CustomerId { get; init; }
    public int ExpertId { get; init; }
    public int RequestId { get; init; }
}