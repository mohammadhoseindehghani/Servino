using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Servino.Domain.Core._common;
using Servino.Domain.Core.CategoryAgg.Contracts.AppService;
using Servino.Domain.Core.CategoryAgg.Dtos;
using System.ComponentModel.DataAnnotations;
using IFileService = Servino.Presentation.RazorPagesUI.Services.File.IFileService;

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

        public string? MessageText { get; private set; }
        public string? MessageType { get; private set; } 

        public async Task OnGet(string? msg, string? text, CancellationToken ct)
        {
            if (!string.IsNullOrEmpty(msg) && !string.IsNullOrEmpty(text))
            {
                MessageType = msg;
                MessageText = text;
            }

            await LoadDataAsync(ct);
        }
        public async Task<IActionResult> OnPostCreateAsync(CancellationToken ct)
        {
            ModelState.Clear();
            if (!TryValidateModel(CreateInput, nameof(CreateInput)))
            {
                return RedirectToPage(new
                {
                    PageNumber,
                    SearchKey,
                    msg = "danger",
                    text = "اطلاعات وارد شده معتبر نیست."
                });
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
                    return RedirectToPage(new
                    {
                        PageNumber,
                        SearchKey,
                        msg = "danger",
                        text = "خطا در آپلود تصویر: " + ex.Message
                    });
                }
            }

            var command = new CategoryDto
            {
                Title = CreateInput.Title,
                ParentId = CreateInput.ParentId,
                ImagePath = imagePath
            };

            var result = await categoryAppService.CreateAsync(command, ct);
            if (!result.IsSuccess && imagePath != null)
            {
                await fileService.DeleteFile(imagePath, ct);
            }

            return RedirectToPage(new
            {
                PageNumber,
                SearchKey,
                msg = result.IsSuccess ? "success" : "danger",
                text = result.Message ?? (result.IsSuccess ? "دسته‌بندی با موفقیت ایجاد شد." : "خطایی رخ داد.")
            });
        }

        public async Task<IActionResult> OnPostEditAsync(CancellationToken ct)
        {
            ModelState.Clear();
            if (!TryValidateModel(EditInput, nameof(EditInput)))
            {
                return RedirectToPage(new
                {
                    PageNumber,
                    SearchKey,
                    msg = "danger",
                    text = "اطلاعات وارد شده برای ویرایش معتبر نیست."
                });
            }

            var current = await categoryAppService.GetByIdAsync(EditInput.Id, ct);
            if (!current.IsSuccess)
            {
                return RedirectToPage(new
                {
                    PageNumber,
                    SearchKey,
                    msg = "danger",
                    text = "دسته‌بندی یافت نشد."
                });
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
                    return RedirectToPage(new
                    {
                        PageNumber,
                        SearchKey,
                        msg = "danger",
                        text = "خطا در آپلود تصویر جدید."
                    });
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

            return RedirectToPage(new
            {
                PageNumber,
                SearchKey,
                msg = result.IsSuccess ? "success" : "danger",
                text = result.Message ?? (result.IsSuccess ? "دسته‌بندی با موفقیت ویرایش شد." : "خطایی رخ داد.")
            });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, CancellationToken ct)
        {
            var cat = await categoryAppService.GetByIdAsync(id, ct);
            var result = await categoryAppService.DeleteAsync(id, ct);

            if (result.IsSuccess && cat.IsSuccess && !string.IsNullOrEmpty(cat.Data.ImagePath))
            {
                await fileService.DeleteFile(cat.Data.ImagePath, ct);
            }

            return RedirectToPage(new
            {
                PageNumber,
                SearchKey,
                msg = result.IsSuccess ? "success" : "danger",
                text = result.Message ?? (result.IsSuccess ? "دسته‌بندی با موفقیت حذف شد." : "خطایی رخ داد.")
            });
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
