using app.Application.DTOs.Common;
using app.Domain.CommentAgg.Enums;

namespace app.Application.DTOs.CommentDTOs;

public record CommentDto : BaseDto
{
    public string Title { get; init; }
    public string Text { get; init; }
    public Rate Rating { get; init; }
    public bool IsApproved { get; init; }
    public DateTime CreatedAt { get; init; } 
    public string CustomerName { get; init; }
    public string ExpertName { get; init; }
}