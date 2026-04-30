using FluentValidation;

namespace app.Application.Features.Users.Commands.CreateUserByAdmin;

public class CreateUserByAdminCommandValidator : AbstractValidator<CreateUserByAdminCommand>
{
    public CreateUserByAdminCommandValidator()
    {
        RuleFor(x => x.Command.Email)
            .NotEmpty().WithMessage("ایمیل الزامی است.")
            .EmailAddress().WithMessage("ایمیل وارد شده معتبر نیست.");

        RuleFor(x => x.Command.Mobile)
            .NotEmpty().WithMessage("شماره موبایل الزامی است.");

        RuleFor(x => x.Command.Password)
            .NotEmpty().WithMessage("رمز عبور الزامی است.")
            .MinimumLength(6).WithMessage("رمز عبور باید حداقل ۶ کاراکتر داشته باشد.");

        RuleFor(x => x.Command.Role)
            .Must(r => new[] { "Expert", "Customer", "Admin" }.Contains(r))
            .WithMessage("نقش نامعتبر است.");
    }
}
