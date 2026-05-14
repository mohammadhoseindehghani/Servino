using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Suggestions.Commands.CreateSuggestion;

public class CreateSuggestionCommandHandler(
    ISuggestionRepository suggestionRepository,
    IRequestRepository requestRepository,
    ILogger<CreateSuggestionCommandHandler> logger,
    IValidator<CreateSuggestionCommand> validator)
    : IRequestHandler<CreateSuggestionCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(CreateSuggestionCommand request,
        CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        try
        {
            var dto = request.Command;

            bool hasSentBefore =
                await suggestionRepository.IsExpertSendSuggestionBeforeAsync(
                    dto.ExpertId, dto.RequestId, ct);

            if (hasSentBefore)
                return Result<bool>.Failure("برای این درخواست قبلاً پیشنهاد ارسال کرده‌اید.");

            var basePrice =
                await requestRepository.GetBasePriceByRequestIdAsync(dto.RequestId, ct);

            if (dto.SuggestedPrice < basePrice)
                return Result<bool>.Failure(
                    $"مبلغ پیشنهادی نمیتواند کمتر از مبلغ پایه: {basePrice} باشد.");

            var created =
                await suggestionRepository.CreateAsync(dto, ct);

            return created
                ? Result<bool>.Success(true, "پیشنهاد با موفقیت ثبت شد.")
                : Result<bool>.Failure("ایجاد پیشنهاد با شکست مواجه شد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "System error in SuggestionAppService.CreateAsync | RequestId: {RequestId} | ExpertId: {ExpertId}",
                request.Command.RequestId,
                request.Command.ExpertId);

            return Result<bool>.Failure("خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }
}
