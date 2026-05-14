using FluentValidation;

namespace app.Application.Features.Province.Commands.DeleteProvince;

public class DeleteProvinceCommandValidator : AbstractValidator<DeleteProvinceCommand>
{
    public DeleteProvinceCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}
