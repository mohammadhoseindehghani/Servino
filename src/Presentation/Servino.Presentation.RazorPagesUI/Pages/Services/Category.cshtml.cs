using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core.CategoryAgg.Contracts.AppService;
using Servino.Domain.Core.CategoryAgg.Dtos;

namespace Servino.Presentation.RazorPagesUI.Pages.Services
{
    public class CategoryModel(ICategoryAppService categoryAppService) : PageModel
    {
        public bool IsServiceMode { get; set; }
        public string CurrentTitle { get; set; }
        public int CurrentId { get; set; }

        public List<CategoryClientDto> SubCategories { get; set; } = [];
        public List<ServiceClientDto> Services { get; set; } = [];
        public List<BreadcrumbDto> Breadcrumbs { get; set; } = [];

        public async Task<IActionResult> OnGet(int id, CancellationToken ct)
        {
            CurrentId = id;

            Breadcrumbs = await categoryAppService.GetBreadcrumbAsync(id, ct);

            CurrentTitle = Breadcrumbs.LastOrDefault()?.Title ?? "دسته‌بندی";
            ViewData["Title"] = CurrentTitle;

            var subs = await categoryAppService.GetCategoriesByParentIdAsync(id, ct);

            if (subs != null! && subs.Any())
            {
                IsServiceMode = false;
                SubCategories = subs;
            }
            else
            {
                IsServiceMode = true;
                Services = await categoryAppService.GetServicesByCategoryIdAsync(id, ct);
            }

            return Page();
        }
    }
}
