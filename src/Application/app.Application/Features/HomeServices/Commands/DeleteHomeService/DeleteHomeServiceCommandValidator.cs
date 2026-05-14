using FluentValidation;

namespace app.Application.Features.HomeServices.Commands.DeleteHomeService;

public class DeleteHomeServiceCommandValidator : AbstractValidator<DeleteHomeServiceCommand>
{
    public DeleteHomeServiceCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}