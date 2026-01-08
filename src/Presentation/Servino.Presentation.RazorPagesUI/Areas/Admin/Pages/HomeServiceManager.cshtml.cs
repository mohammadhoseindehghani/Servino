using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Servino.Domain.Core._common;
using Servino.Domain.Core.CategoryAgg.Contracts.AppService;
using Servino.Domain.Core.HomeServiceAgg.Contracts.AppService;
using Servino.Domain.Core.HomeServiceAgg.Dtos;
using Servino.Presentation.RazorPagesUI.Services.File;

namespace Servino.Presentation.RazorPagesUI.Areas.Admin.Pages
{
    public class HomeServiceManagerModel(
        IHomeServiceAppService homeServiceAppService,
        ICategoryAppService categoryAppService,
        IFileService fileService) : PageModel
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

        [TempData] public string? SuccessMessage { get; set; }
        [TempData] public string? ErrorMessage { get; set; }

        public async Task OnGet(CancellationToken ct)
        {
            await LoadDataAsync(ct);
        }

        public async Task<IActionResult> OnPostCreateAsync(CancellationToken ct)
        {

            string? imagePath = null;
            if (CreateInput.ImageFile != null)
            {
                imagePath = await fileService.Upload(CreateInput.ImageFile, "services", ct);
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

            if (result.IsSuccess)
                SuccessMessage = result.Message;
            else
            {
                if (imagePath != null) await fileService.DeleteFile(imagePath, ct);
                ErrorMessage = result.Message;
            }

            return RedirectToPage(new { PageNumber, SearchKey });
        }

        public async Task<IActionResult> OnPostEditAsync(CancellationToken ct)
        {
            var currentServiceResult = await homeServiceAppService.GetByIdAsync(EditInput.Id, ct);
            if (!currentServiceResult.IsSuccess)
            {
                ErrorMessage = "سرویس یافت نشد.";
                return RedirectToPage(new { PageNumber, SearchKey });
            }

            string? newImagePath = currentServiceResult.Data.ImagePath;

            if (EditInput.ImageFile != null)
            {
                newImagePath = await fileService.Upload(EditInput.ImageFile, "services", ct);

                await fileService.DeleteFile(currentServiceResult.Data.ImagePath, ct);
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

            if (result.IsSuccess)
                SuccessMessage = result.Message;
            else
                ErrorMessage = result.Message;

            return RedirectToPage(new { PageNumber, SearchKey });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, CancellationToken ct)
        {
            var serviceResult = await homeServiceAppService.GetByIdAsync(id, ct);

            var result = await homeServiceAppService.DeleteAsync(id, ct);

            if (result.IsSuccess)
            {
                if (serviceResult.IsSuccess && !string.IsNullOrEmpty(serviceResult.Data.ImagePath))
                {
                    await fileService.DeleteFile(serviceResult.Data.ImagePath, ct);
                }
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
            var catPagination = new PaginationRequestDto { PageSize = 100 };
            var cats = await categoryAppService.GetAllAsync(catPagination, ct);

            if (cats == null) cats = new();
            Categories = new SelectList(cats, "Id", "Title");
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
