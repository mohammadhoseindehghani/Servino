using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core.SuggestionAgg.Contracts.AppService;
using Servino.Domain.Core.SuggestionAgg.Dtos;
using Servino.Presentation.RazorPagesUI.Extensions;
using Servino.Framework.Extensions;


namespace Servino.Presentation.RazorPagesUI.Areas.Expert.Pages
{

    [Authorize(Roles = "Expert")]
    public class CreateSuggestionModel(
        ISuggestionAppService suggestionAppService) : PageModel
    {
        [BindProperty]
        public CreateSuggestionDto Command { get; set; } = new();

        [BindProperty]
        public string SuggestedDate { get; set; }


        public void OnGet(int requestId)
        {
            Command.RequestId = requestId;
            Command.ExpertId = User.GetUserId();
        }

        public async Task<IActionResult> OnPost(CancellationToken ct)
        {
            Command.ExpertId = User.GetExpertId();

            if (!ModelState.IsValid)
                return Page();

            if (string.IsNullOrWhiteSpace(SuggestedDate))
            {
                ModelState.AddModelError(nameof(SuggestedDate),
                    "لطفاً تاریخ پیشنهادی را انتخاب کنید");
                return Page();
            }

            DateTime gregorianDate;
            try
            {
                gregorianDate = SuggestedDate.ToGregorianDateTime()!.Value;
            }
            catch
            {
                ModelState.AddModelError(nameof(SuggestedDate),
                    "فرمت تاریخ نامعتبر است");
                return Page();
            }

            var createDto = new CreateSuggestionDto
            {
                RequestId = Command.RequestId,
                ExpertId = Command.ExpertId,
                SuggestedPrice = Command.SuggestedPrice,
                EstimatedDurationHours = Command.EstimatedDurationHours,
                Note = Command.Note,
                SuggestedDate = gregorianDate
            };

            var result = await suggestionAppService.CreateAsync(createDto, ct);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return Page();
            }

            return RedirectToPage("./AvailableRequests");
        }


    }

}
