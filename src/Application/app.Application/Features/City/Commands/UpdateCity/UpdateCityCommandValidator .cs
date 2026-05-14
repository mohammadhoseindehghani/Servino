using FluentValidation;

namespace app.Application.Features.City.Commands.UpdateCity;

public class UpdateCityCommandValidator : AbstractValidator<UpdateCityCommand>
{
    public UpdateCityCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.ProvinceId)
            .GreaterThan(0);
    }
}