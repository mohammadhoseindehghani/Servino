using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core._common;
using Servino.Domain.Core.CommentAgg.Contracts.AppService;
using Servino.Domain.Core.CommentAgg.Dtos;

namespace Servino.Presentation.RazorPagesUI.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class CommentManagerModel(ICommentAppService commentService) : PageModel
    {
        public List<CommentDto> Comments { get; set; } = [];

        [BindProperty(SupportsGet = true)]
        public string? SearchKey { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? MessageText { get; private set; }
        public string? MessageType { get; private set; } 

        public async Task OnGet(string? msg, string? text, CancellationToken ct)
        {
            if (!string.IsNullOrEmpty(msg) && !string.IsNullOrEmpty(text))
            {
                MessageType = msg;
                MessageText = text;
            }

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
                MessageType = "danger";
                MessageText = result.Message;
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, CancellationToken ct)
        {
            var result = await commentService.DeleteAsync(id, ct);

            return RedirectToPage(new
            {
                PageNumber,
                SearchKey,
                msg = result.IsSuccess ? "success" : "danger",
                text = result.Message ?? (result.IsSuccess ? "نظر با موفقیت حذف شد." : "خطایی رخ داد.")
            });
        }

        public async Task<IActionResult> OnPostChangeStatusAsync(int id, bool isApproved, CancellationToken ct)
        {
            var result = await commentService.ChangeApprovalStatusAsync(id, isApproved, ct);

            return RedirectToPage(new
            {
                PageNumber,
                SearchKey,
                msg = result.IsSuccess ? "success" : "danger",
                text = result.Message ?? (result.IsSuccess ? "وضعیت نظر با موفقیت تغییر کرد." : "خطایی رخ داد.")
            });
        }
    }
}
