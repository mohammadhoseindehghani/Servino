using FluentValidation;

namespace app.Application.Features.Users.Commands.ChangeUserBalance;

public class ChangeUserBalanceCommandValidator : AbstractValidator<ChangeUserBalanceCommand>
{
    public ChangeUserBalanceCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("شناسه کاربر نامعتبر است.");

        RuleFor(x => x.Amount)
            .NotEqual(0).WithMessage("مبلغ تغییر موجودی نمی‌تواند صفر باشد.");
    }
}
