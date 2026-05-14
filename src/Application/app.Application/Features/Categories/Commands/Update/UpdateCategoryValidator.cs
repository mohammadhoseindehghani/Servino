using FluentValidation;

namespace app.Application.Features.Categories.Commands.Update;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان الزامی است.")
            .MinimumLength(3).WithMessage("عنوان نمیتواند کمتر از 3 کاراکتر باشد.");
    }
}