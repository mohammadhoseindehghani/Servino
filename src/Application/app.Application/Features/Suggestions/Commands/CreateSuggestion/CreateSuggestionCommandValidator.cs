using FluentValidation;

namespace app.Application.Features.Suggestions.Commands.CreateSuggestion;

public class CreateSuggestionCommandValidator
    : AbstractValidator<CreateSuggestionCommand>
{
    public CreateSuggestionCommandValidator()
    {
        RuleFor(x => x.Command.RequestId)
            .GreaterThan(0)
            .WithMessage("درخواست نامعتبر است.");

        RuleFor(x => x.Command.ExpertId)
            .GreaterThan(0)
            .WithMessage("متخصص امکان ارسال پیشنهاد را ندارد.");

        RuleFor(x => x.Command.EstimatedDurationHours)
            .GreaterThan(0)
            .WithMessage("طول ساعت کاری نامعتبر است.");

        RuleFor(x => x.Command.SuggestedDate)
            .Must(d => d > DateTime.Now)
            .WithMessage("تاریخ پیشنهادی نمیتواند در گذشته باشد.");
    }
}
