namespace app.Application.Common;

public class PaginationRequestDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchKey { get; set; } 
}