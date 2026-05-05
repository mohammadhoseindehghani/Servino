namespace app.WebApi.Model;

public class UpdateCategoryDto
{
    public string Title { get; set; }
    public IFormFile Image { get; set; }
    public int? ParentId { get; set; }
}