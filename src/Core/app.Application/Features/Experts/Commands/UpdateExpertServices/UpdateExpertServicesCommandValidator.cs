using FluentValidation;

namespace app.Application.Features.Experts.Commands.UpdateExpertServices;

public class UpdateExpertServicesCommandValidator
    : AbstractValidator<UpdateExpertServicesCommand>
{
    public UpdateExpertServicesCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("شناسه کاربر نامعتبر است.");

        RuleFor(x => x.SelectedIds)
            .NotNull()
            .WithMessage("لیست سرویس‌ها نمی‌تواند خالی باشد.");
    }
}
