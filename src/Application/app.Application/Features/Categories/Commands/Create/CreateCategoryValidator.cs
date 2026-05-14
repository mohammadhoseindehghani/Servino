using FluentValidation;

namespace app.Application.Features.Categories.Commands.Create;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان الزامی است.")
            .MinimumLength(3).WithMessage("عنوان نمیتواند کمتر از 3 کاراکتر باشد.");
    }
}