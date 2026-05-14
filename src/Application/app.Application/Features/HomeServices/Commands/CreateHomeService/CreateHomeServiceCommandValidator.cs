using FluentValidation;

namespace app.Application.Features.HomeServices.Commands.CreateHomeService;

public class CreateHomeServiceCommandValidator : AbstractValidator<CreateHomeServiceCommand>
{
    public CreateHomeServiceCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان خدمت نمی‌تواند خالی باشد.")
            .MaximumLength(200);

        RuleFor(x => x.BasePrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("قیمت پایه نمی‌تواند منفی باشد.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("انتخاب دسته‌بندی الزامی است.");
    }
}
