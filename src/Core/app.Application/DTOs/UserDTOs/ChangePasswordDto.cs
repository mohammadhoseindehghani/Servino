namespace app.Application.DTOs.UserDTOs;

public record ChangePasswordDto
{
    public int UserId { get; init; }          
    public string CurrentPassword { get; init; } 
    public string NewPassword { get; init; } 
}