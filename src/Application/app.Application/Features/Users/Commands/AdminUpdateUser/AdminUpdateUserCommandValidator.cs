using FluentValidation;

namespace app.Application.Features.Users.Commands.AdminUpdateUser;

public class AdminUpdateUserCommandValidator : AbstractValidator<AdminUpdateUserCommand>
{
    public AdminUpdateUserCommandValidator()
    {
        RuleFor(x => x.Command.Id)
            .GreaterThan(0).WithMessage("شناسه کاربر نامعتبر است.");

        RuleFor(x => x.Command.Mobile)
            .NotEmpty().WithMessage("شماره موبایل الزامی است.");

        RuleFor(x => x.Command.FirstName)
            .NotEmpty().WithMessage("نام الزامی است.");

        RuleFor(x => x.Command.LastName)
            .NotEmpty().WithMessage("نام خانوادگی الزامی است.");

        RuleFor(x => x.Command.Role)
            .NotEmpty().WithMessage("نقش کاربر الزامی است.");
    }
}
