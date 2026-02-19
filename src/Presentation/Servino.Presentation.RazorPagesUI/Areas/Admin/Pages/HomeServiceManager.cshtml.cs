using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Servino.Domain.Core._common;
using Servino.Domain.Core.CategoryAgg.Contracts.AppService;
using Servino.Domain.Core.CategoryAgg.Dtos;
using Servino.Domain.Core.HomeServiceAgg.Contracts.AppService;
using Servino.Domain.Core.HomeServiceAgg.Dtos;
using IFileService = Servino.Presentation.RazorPagesUI.Services.File.IFileService;

namespace Servino.Presentation.RazorPagesUI.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class HomeServiceManagerModel(
        IHomeServiceAppService homeServiceAppService,
        ICategoryAppService categoryAppService,
        IFileService fileService)
        : PageModel
    {
        public List<HomeServiceSummaryDto> Services { get; set; } = [];
        public SelectList Categories { get; set; } 

        [BindProperty(SupportsGet = true)]
        public string? SearchKey { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        [BindProperty]
        public CreateHomeServiceModel CreateInput { get; set; } = new();

        [BindProperty]
        public EditHomeServiceModel EditInput { get; set; } = new();

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
            string? imagePath = null;
            if (CreateInput.ImageFile != null)
            {
                try
                {
                    imagePath = await fileService.Upload(CreateInput.ImageFile, "services", ct);
                }
                catch
                {
                    return RedirectToPage(new { PageNumber, SearchKey, msg = "danger", text = "خطا در آپلود تصویر." });
                }
            }

            var command = new HomeServiceDto
            {
                Title = CreateInput.Title,
                ShortDescription = CreateInput.ShortDescription,
                BasePrice = CreateInput.BasePrice,
                CategoryId = CreateInput.CategoryId,
                ImagePath = imagePath
            };

            var result = await homeServiceAppService.CreateAsync(command, ct);

            if (!result.IsSuccess && imagePath != null)
                await fileService.DeleteFile(imagePath, ct);

            return RedirectToPage(new
            {
                PageNumber,
                SearchKey,
                msg = result.IsSuccess ? "success" : "danger",
                text = result.Message ?? (result.IsSuccess ? "خدمت با موفقیت ایجاد شد." : "خطایی رخ داد.")
            });
        }

        public async Task<IActionResult> OnPostEditAsync(CancellationToken ct)
        {
            var current = await homeServiceAppService.GetByIdAsync(EditInput.Id, ct);
            if (!current.IsSuccess)
            {
                return RedirectToPage(new { PageNumber, SearchKey, msg = "danger", text = "سرویس یافت نشد." });
            }

            string? newImagePath = current.Data.ImagePath;
            if (EditInput.ImageFile != null)
            {
                try
                {
                    newImagePath = await fileService.Upload(EditInput.ImageFile, "services", ct);
                    if (!string.IsNullOrEmpty(current.Data.ImagePath))
                        await fileService.DeleteFile(current.Data.ImagePath, ct);
                }
                catch
                {
                    return RedirectToPage(new { PageNumber, SearchKey, msg = "danger", text = "خطا در آپلود تصویر جدید." });
                }
            }

            var command = new HomeServiceDto
            {
                Id = EditInput.Id,
                Title = EditInput.Title,
                ShortDescription = EditInput.ShortDescription,
                BasePrice = EditInput.BasePrice,
                CategoryId = EditInput.CategoryId,
                ImagePath = newImagePath
            };

            var result = await homeServiceAppService.UpdateAsync(command, ct);

            return RedirectToPage(new
            {
                PageNumber,
                SearchKey,
                msg = result.IsSuccess ? "success" : "danger",
                text = result.Message ?? (result.IsSuccess ? "خدمت با موفقیت ویرایش شد." : "خطایی رخ داد.")
            });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, CancellationToken ct)
        {
            var serviceResult = await homeServiceAppService.GetByIdAsync(id, ct);
            var result = await homeServiceAppService.DeleteAsync(id, ct);

            if (result.IsSuccess && serviceResult.IsSuccess && !string.IsNullOrEmpty(serviceResult.Data.ImagePath))
            {
                await fileService.DeleteFile(serviceResult.Data.ImagePath, ct);
            }

            return RedirectToPage(new
            {
                PageNumber,
                SearchKey,
                msg = result.IsSuccess ? "success" : "danger",
                text = result.Message ?? (result.IsSuccess ? "خدمت با موفقیت حذف شد." : "خطایی رخ داد.")
            });
        }

        public async Task<IActionResult> OnGetRowDataAsync(int id, CancellationToken ct)
        {
            var result = await homeServiceAppService.GetByIdAsync(id, ct);
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

            var result = await homeServiceAppService.GetAllAsync(pagination, ct);
            Services = result.IsSuccess ? result.Data! : [];

            var cats = await categoryAppService.GetAllAsync(new PaginationRequestDto { PageSize = 100 }, ct);
            Categories = new SelectList(cats ?? new List<CategorySummaryDto>(), "Id", "Title");
        }

        public class CreateHomeServiceModel
        {
            public string Title { get; set; }
            public string? ShortDescription { get; set; }
            public int CategoryId { get; set; }
            public decimal BasePrice { get; set; }
            public IFormFile? ImageFile { get; set; }
        }

        public class EditHomeServiceModel : CreateHomeServiceModel
        {
            public int Id { get; set; }
        }
    }
}
