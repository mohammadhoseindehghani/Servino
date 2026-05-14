using FluentValidation;

namespace app.Application.Features.Users.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileCommandValidator()
    {
        RuleFor(x => x.Command.Id)
            .GreaterThan(0)
            .WithMessage("شناسه کاربر نامعتبر است.");

        RuleFor(x => x.Command.FirstName)
            .NotEmpty()
            .WithMessage("نام الزامی است.");

        RuleFor(x => x.Command.LastName)
            .NotEmpty()
            .WithMessage("نام خانوادگی الزامی است.");
    }
}
