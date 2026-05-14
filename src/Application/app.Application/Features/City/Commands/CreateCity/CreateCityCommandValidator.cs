using FluentValidation;

namespace app.Application.Features.City.Commands.CreateCity;

public class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
{
    public CreateCityCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان شهر الزامی است.")
            .MaximumLength(200);

        RuleFor(x => x.ProvinceId)
            .GreaterThan(0).WithMessage("استان معتبر نیست.");
    }
}