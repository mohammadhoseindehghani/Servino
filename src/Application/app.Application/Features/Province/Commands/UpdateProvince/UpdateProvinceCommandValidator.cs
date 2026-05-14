using FluentValidation;

namespace app.Application.Features.Province.Commands.UpdateProvince;

public class UpdateProvinceCommandValidator : AbstractValidator<UpdateProvinceCommand>
{
    public UpdateProvinceCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);
    }
}
