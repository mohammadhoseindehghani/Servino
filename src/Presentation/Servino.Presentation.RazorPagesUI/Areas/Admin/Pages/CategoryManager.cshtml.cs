using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Servino.Domain.Core._common;
using Servino.Domain.Core.CategoryAgg.Contracts.AppService;
using Servino.Domain.Core.CategoryAgg.Dtos;
using Servino.Presentation.RazorPagesUI.Services.File;

namespace Servino.Presentation.RazorPagesUI.Areas.Admin.Pages
{
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

        [TempData] public string? SuccessMessage { get; set; }
        [TempData] public string? ErrorMessage { get; set; }

        public async Task OnGet(CancellationToken ct)
        {
            await LoadDataAsync(ct);
        }

        public async Task<IActionResult> OnPostCreateAsync(CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "اطلاعات وارد شده معتبر نیست.";
                await LoadDataAsync(ct);
                return Page();
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
                    await LoadDataAsync(ct);
                    return Page();
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
                return RedirectToPage(new { PageNumber, SearchKey });
            }
            else
            {
                if (imagePath != null) await fileService.DeleteFile(imagePath, ct);

                ErrorMessage = result.Message;
                await LoadDataAsync(ct);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, CancellationToken ct)
        {
            var result = await categoryAppService.DeleteAsync(id, ct);

            if (result.IsSuccess)
                SuccessMessage = result.Message;
            else
                ErrorMessage = result.Message;

            return RedirectToPage(new { PageNumber, SearchKey });
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
            var allParents = await categoryAppService.GetAllAsync(new PaginationRequestDto { PageSize = 100 }, ct);
            ParentCategories = new SelectList(allParents, nameof(CategorySummaryDto.Id), nameof(CategorySummaryDto.Title));
        }

        public class CreateCategoryModel
        {
            public string Title { get; set; }
            public int? ParentId { get; set; }
            public IFormFile? ImageFile { get; set; }
            public bool IsActive { get; set; } = true;
        }
    }
}
