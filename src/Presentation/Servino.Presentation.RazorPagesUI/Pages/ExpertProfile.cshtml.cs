using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core._common;
using Servino.Domain.Core.CommentAgg.Contracts.AppService;
using Servino.Domain.Core.CommentAgg.Dtos;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Dtos;

namespace Servino.Presentation.RazorPagesUI.Pages;

public class ExpertProfileModel(IExpertAppService expertAppService, ICommentAppService commentAppService) : PageModel
{
    public ExpertProfileDto ExpertProfile { get; set; } = new();
    public List<CommentDto> Comments { get; set; } = new();
    public List<ExpertServiceItemDto> ExpertServices { get; set; } = new();
    public PaginationRequestDto Pagination { get; set; } = new();
    public int TotalComments { get; set; }

    public async Task<IActionResult> OnGetAsync(int expertId, int pageNumber = 1, int pageSize = 5, CancellationToken ct = default)
    {
        Pagination.PageNumber = pageNumber;
        Pagination.PageSize = pageSize;

        var expertResult = await expertAppService.GetByExpertId(expertId, ct);
        if (!expertResult.IsSuccess)
            return RedirectToPage("/Error");

        ExpertProfile = expertResult.Data;

        ExpertServices = await expertAppService
            .GetExpertServicesByExpertIdAsync(expertId, ct);

        var commentResult = await commentAppService.GetApprovedByExpertIdAsync(expertId, Pagination, ct);
        if (commentResult.IsSuccess)
        {
            Comments = commentResult.Data;
            TotalComments = 5; 
        }

        return Page();
    }

    public int TotalPages => (int)Math.Ceiling((double)TotalComments / Pagination.PageSize);
}

