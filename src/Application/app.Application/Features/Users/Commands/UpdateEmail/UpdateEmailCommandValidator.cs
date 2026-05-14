using FluentValidation;

namespace app.Application.Features.Users.Commands.UpdateEmail;

public class UpdateEmailCommandValidator : AbstractValidator<UpdateEmailCommand>
{
    public UpdateEmailCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("شناسه کاربر نامعتبر است.");

        RuleFor(x => x.NewEmail)
            .NotEmpty().WithMessage("ایمیل جدید الزامی است.")
            .EmailAddress().WithMessage("ایمیل جدید نامعتبر است.");
    }
}
