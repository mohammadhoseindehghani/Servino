using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.DTOs.RequestDTOs;
using app.Application.DTOs.SuggestionDTOs;
using app.Application.Features.Requests.Commands.SelectExpert;
using app.Domain.SuggestionAgg.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Requests.Commands.SelectExpert;

public class SelectExpertCommandHandler(
    IRequestRepository requestRepository,
    ISuggestionRepository suggestionRepository,
    IUserRepository userRepository,
    ILogger<SelectExpertCommandHandler> logger)
    : IRequestHandler<SelectExpertCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(SelectExpertCommand command, CancellationToken ct)
    {
        try
        {
            var request = await requestRepository.GetByIdAsync(command.RequestId, ct);
            if (request == null) return Result<bool>.Failure("درخواست یافت نشد.");

            if (request.CustomerUserId != command.CustomerId)
                return Result<bool>.Failure("دسترسی غیرمجاز.");

            var suggestion = await suggestionRepository.GetByIdAsync(command.SuggestionId, ct);
            if (suggestion == null)
                return Result<bool>.Failure("پیشنهاد یافت نشد.");

            var user = await userRepository.GetByIdAsync(request.CustomerUserId, ct);

            if (user.BalanceAmount < suggestion.SuggestedPrice)
                return Result<bool>.Failure("موجودی کافی نیست.");

            suggestion.Status = SuggestionStatus.Accepted;

            await suggestionRepository.UpdateAsync(new UpdateSuggestionDto
            {
                Id = suggestion.Id,
                Status = SuggestionStatus.Accepted,
                SuggestedPrice = suggestion.SuggestedPrice,
                SuggestedDate = suggestion.SuggestedDate,
                EstimatedDurationHours = suggestion.EstimatedDurationHours,
                Note = suggestion.Note
            }, ct);

            await requestRepository.UpdateAsync(new UpdateRequestDto
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
