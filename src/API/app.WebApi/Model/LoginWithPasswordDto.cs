namespace app.WebApi.Model;

public record LoginWithPasswordDto
{
    public string UserName { get; init; }
    public string Password { get; init; }
}