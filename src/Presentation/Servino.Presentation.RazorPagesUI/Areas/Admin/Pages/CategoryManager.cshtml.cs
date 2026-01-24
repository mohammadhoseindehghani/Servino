using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Servino.Domain.Core._common;
using Servino.Domain.Core.CategoryAgg.Contracts.AppService;
using Servino.Domain.Core.CategoryAgg.Dtos;
using Servino.Presentation.RazorPagesUI.Services.File;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace Servino.Presentation.RazorPagesUI.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class CategoryManagerModel(
           ICategoryAppService categoryAppService,
           IFileService fileService) : PageModel
    {
        public List<CategorySummaryDto> Categories { get; set; } = [];
        public SelectList ParentCategories { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchKey { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        [BindProperty]
        public CreateCategoryModel CreateInput { get; set; } = new();

        [BindProperty]
        public EditCategoryModel EditInput { get; set; } = new();

        [TempData] public string? SuccessMessage { get; set; }
        [TempData] public string? ErrorMessage { get; set; }

        public async Task OnGet(CancellationToken ct)
        {
            await LoadDataAsync(ct);
        }

        public async Task<IActionResult> OnPostCreateAsync(CancellationToken ct)
        {
            ModelState.Clear();

            if (!TryValidateModel(CreateInput, nameof(CreateInput)))
            {
                ErrorMessage = "اطلاعات وارد شده معتبر نیست.";
                return RedirectToPage(new { PageNumber, SearchKey });
            }

            string? imagePath = null;
            if (CreateInput.ImageFile != null)
            {
                try
                {
                    imagePath = await fileService.Upload(CreateInput.ImageFile, "categories", ct);
                }
                catch (Exception ex)
                {
                    ErrorMessage = "خطا در آپلود تصویر: " + ex.Message;
                    return RedirectToPage(new { PageNumber, SearchKey });
                }
            }

            var command = new CategoryDto
            {
                Title = CreateInput.Title,
                ParentId = CreateInput.ParentId,
                ImagePath = imagePath
            };

            var result = await categoryAppService.CreateAsync(command, ct);

            if (result.IsSuccess)
            {
                SuccessMessage = result.Message;
            }
            else
            {
                if (imagePath != null)
                    await fileService.DeleteFile(imagePath, ct);

                ErrorMessage = result.Message;
            }

            return RedirectToPage(new { PageNumber, SearchKey });
        }

        public async Task<IActionResult> OnPostEditAsync(CancellationToken ct)
        {
            ModelState.Clear();

            if (!TryValidateModel(EditInput, nameof(EditInput)))
            {
                ErrorMessage = "اطلاعات وارد شده برای ویرایش معتبر نیست.";
                return RedirectToPage(new { PageNumber, SearchKey });
            }

            var current = await categoryAppService.GetByIdAsync(EditInput.Id, ct);
            if (!current.IsSuccess)
            {
                ErrorMessage = "دسته‌بندی یافت نشد.";
                return RedirectToPage(new { PageNumber, SearchKey });
            }

            string? newImage = current.Data.ImagePath;

            if (EditInput.ImageFile != null)
            {
                try
                {
                    newImage = await fileService.Upload(EditInput.ImageFile, "categories", ct);

                    if (!string.IsNullOrEmpty(current.Data.ImagePath))
                        await fileService.DeleteFile(current.Data.ImagePath, ct);
                }
                catch
                {
                    ErrorMessage = "خطا در آپلود تصویر جدید.";
                    return RedirectToPage(new { PageNumber, SearchKey });
                }
            }

            var command = new CategoryDto
            {
                Id = EditInput.Id,
                Title = EditInput.Title,
                ParentId = EditInput.ParentId,
                ImagePath = newImage
            };

            var result = await categoryAppService.UpdateAsync(command, ct);

            if (result.IsSuccess)
                SuccessMessage = result.Message;
            else
                ErrorMessage = result.Message;

            return RedirectToPage(new { PageNumber, SearchKey });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, CancellationToken ct)
        {
            var cat = await categoryAppService.GetByIdAsync(id, ct);
            var result = await categoryAppService.DeleteAsync(id, ct);

            if (result.IsSuccess)
            {
                if (cat.IsSuccess && !string.IsNullOrEmpty(cat.Data.ImagePath))
                    await fileService.DeleteFile(cat.Data.ImagePath, ct);

                SuccessMessage = result.Message;
            }
            else
            {
                ErrorMessage = result.Message;
            }

            return RedirectToPage(new { PageNumber, SearchKey });
        }

        public async Task<IActionResult> OnGetRowDataAsync(int id, CancellationToken ct)
        {
            var result = await categoryAppService.GetByIdAsync(id, ct);
            if (!result.IsSuccess) return NotFound();
            return new JsonResult(result.Data);
        }

        private async Task LoadDataAsync(CancellationToken ct)
        {
            var pagination = new PaginationRequestDto
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
                SearchKey = SearchKey
            };

            Categories = await categoryAppService.GetAllAsync(pagination, ct);
            var allParents = await categoryAppService.GetAllAsync(new PaginationRequestDto { PageSize = 200 }, ct);
            ParentCategories = new SelectList(allParents, nameof(CategorySummaryDto.Id), nameof(CategorySummaryDto.Title));
        }

        public class CreateCategoryModel
        {
            [Required(ErrorMessage = "عنوان الزامی است")]
            public string? Title { get; set; }
            public int? ParentId { get; set; }
            public IFormFile? ImageFile { get; set; }
        }

        public class EditCategoryModel
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "عنوان الزامی است")]
            public string? Title { get; set; }

            public int? ParentId { get; set; }
            public IFormFile? ImageFile { get; set; }
        }
    }
}
