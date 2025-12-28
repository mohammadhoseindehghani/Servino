using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core._common;
using Servino.Domain.Core.CommentAgg.Contracts.AppService;
using Servino.Domain.Core.CommentAgg.Dtos;

namespace Servino.Presentation.RazorPagesUI.Areas.Admin.Pages
{
    public class CommentManagerModel(ICommentAppService commentService) : PageModel
    {
        public List<CommentDto> Comments { get; set; } = [];

        [BindProperty(SupportsGet = true)]
        public string? SearchKey { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        [TempData]
        public string? SuccessMessage { get; set; }
        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task OnGet(CancellationToken ct)
        {
            var pagination = new PaginationRequestDto
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
                SearchKey = SearchKey
            };

            var result = await commentService.GetAllAsync(pagination, ct);

            if (result.IsSuccess)
            {
                Comments = result.Data ?? [];
            }
            else
            {
                ErrorMessage = result.Message;
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, CancellationToken ct)
        {
            var result = await commentService.DeleteAsync(id, ct);
            if (result.IsSuccess)
            {
                SuccessMessage = result.Message;
            }
            else
            {
                ErrorMessage = result.Message;
            }
            return RedirectToPage(new { PageNumber, SearchKey });
        }

        public async Task<IActionResult> OnPostChangeStatusAsync(int id, bool isApproved, CancellationToken ct)
        {
            var result = await commentService.ChangeApprovalStatusAsync(id, isApproved, ct);
            if (result.IsSuccess)
            {
                SuccessMessage = result.Message;
            }
            else
            {
                ErrorMessage = result.Message;
            }
            return RedirectToPage(new { PageNumber, SearchKey });
        }
    }
}
