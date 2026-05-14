using FluentValidation;

namespace app.Application.Features.HomeServices.Commands.UpdateHomeService;

public class UpdateHomeServiceCommandValidator : AbstractValidator<UpdateHomeServiceCommand>
{
    public UpdateHomeServiceCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("شناسه خدمت نامعتبر است.");

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.BasePrice)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.CategoryId)
            .GreaterThan(0);
    }
}
