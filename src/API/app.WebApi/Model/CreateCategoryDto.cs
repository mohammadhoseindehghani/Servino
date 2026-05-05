using System.ComponentModel.DataAnnotations;

namespace app.WebApi.Model;

public class CreateCategoryDto
{
    [Required(ErrorMessage = "عنوان دسته بندی اجباری است.")]
    [MinLength(3, ErrorMessage = "عنوان دسته بندی حداقل باید 3 کاراکتر باشد.")]
    public string Title { get; set; }
    public int? ParentId { get; set; }
    public IFormFile? Image { get; set; }
}