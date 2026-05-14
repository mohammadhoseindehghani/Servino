using FluentValidation;

namespace app.Application.Features.Users.Commands.UpdateProfileImage;

public class UpdateProfileImageCommandValidator : AbstractValidator<UpdateProfileImageCommand>
{
    public UpdateProfileImageCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("شناسه کاربر نامعتبر است.");

        RuleFor(x => x.Path)
            .NotNull().WithMessage("انتخاب تصویر پروفایل الزامی است.");
    }
}
