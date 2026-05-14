using FluentValidation;

namespace app.Application.Features.City.Commands.DeleteCity;

public class DeleteCityCommandValidator : AbstractValidator<DeleteCityCommand>
{
    public DeleteCityCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}
