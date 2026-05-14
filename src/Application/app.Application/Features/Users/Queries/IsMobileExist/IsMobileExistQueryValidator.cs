using FluentValidation;

namespace app.Application.Features.Users.Queries.IsMobileExist;

public class IsMobileExistQueryValidator : AbstractValidator<IsMobileExistQuery>
{
    public IsMobileExistQueryValidator()
    {
        RuleFor(x => x.Mobile)
            .NotEmpty().WithMessage("شماره موبایل الزامی است.");
    }
}
