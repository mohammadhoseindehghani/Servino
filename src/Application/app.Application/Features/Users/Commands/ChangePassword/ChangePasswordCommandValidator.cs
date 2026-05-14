using FluentValidation;

namespace app.Application.Features.Users.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.Command.UserId)
            .GreaterThan(0)
            .WithMessage("شناسه کاربر نامعتبر است.");

        RuleFor(x => x.Command.CurrentPassword)
            .NotEmpty()
            .WithMessage("رمز عبور فعلی الزامی است.");

        RuleFor(x => x.Command.NewPassword)
            .NotEmpty()
            .MinimumLength(6)
            .WithMessage("رمز عبور جدید باید حداقل ۶ کاراکتر باشد.");
    }
}
