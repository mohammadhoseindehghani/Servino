using FluentValidation;

namespace app.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Command.Email)
            .NotEmpty().WithMessage("ایمیل الزامی است.")
            .EmailAddress().WithMessage("ایمیل نامعتبر است.");

        RuleFor(x => x.Command.Password)
            .MinimumLength(6).WithMessage("پسورد باید حداقل ۶ کاراکتر داشته باشد.");

        RuleFor(x => x.Command.PhoneNumber)
            .NotEmpty().WithMessage("شماره موبایل الزامی است.");

        RuleFor(x => x.Command.Role)
            .Must(r => new[] { "Expert", "Customer" }.Contains(r))
            .WithMessage("نقش کاربر نامعتبر است.");
    }
}
