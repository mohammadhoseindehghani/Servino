using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.RequestAgg;
using app.Application.Contracts.Contracts.Services.Suggestion;
using app.Application.Contracts.Contracts.Services.UserAgg;
using app.Application.Contracts.DTOs.RequestDTOs;
using app.Application.Contracts.DTOs.SuggestionDTOs;
using app.Domain.SuggestionAgg.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Requests.Commands.SelectExpert;

public class SelectExpertCommandHandler(
    IRequestService requestService,
    ISuggestionService suggestionService,
    IUserService userService,
    ILogger<SelectExpertCommandHandler> logger)
    : IRequestHandler<SelectExpertCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(SelectExpertCommand command, CancellationToken ct)
    {
        try
        {
            var request = await requestService.GetByIdAsync(command.RequestId, ct);
            if (request == null) return Result<bool>.Failure("درخواست یافت نشد.");

            if (request.CustomerUserId != command.CustomerId)
                return Result<bool>.Failure("دسترسی غیرمجاز.");

            var suggestion = await suggestionService.GetByIdAsync(command.SuggestionId, ct);
            if (suggestion == null)
                return Result<bool>.Failure("پیشنهاد یافت نشد.");

            var user = await userService.GetByIdAsync(request.CustomerUserId, ct);

            if (user.BalanceAmount < suggestion.SuggestedPrice)
                return Result<bool>.Failure("موجودی کافی نیست.");

            suggestion.Status = SuggestionStatus.Accepted;

            await suggestionService.UpdateAsync(new UpdateSuggestionDto
            {
                Id = suggestion.Id,
                Status = SuggestionStatus.Accepted,
                SuggestedPrice = suggestion.SuggestedPrice,
                SuggestedDate = suggestion.SuggestedDate,
                EstimatedDurationHours = suggestion.EstimatedDurationHours,
                Note = suggestion.Note
            }, ct);

            await requestService.UpdateAsync(new UpdateRequestDto
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                Address = request.Address,
                CityId = request.CityId,
                DateRequired = request.DateRequired,
                Status = RequestStatus.Started,
                WinnerSuggestionId = suggestion.Id
            }, ct);

            return Result<bool>.Success(true, "متخصص انتخاب شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error selecting expert");
            return Result<bool>.Failure("خطای سیستمی");
        }
    }
}
