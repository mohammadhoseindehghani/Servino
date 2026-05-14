using FluentValidation;

namespace app.Application.Features.Experts.Queries.GetExpertServicesForEdit;

public class GetExpertServicesForEditQueryValidator
    : AbstractValidator<GetExpertServicesForEditQuery>
{
    public GetExpertServicesForEditQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("شناسه کاربر نامعتبر است.");
    }
}
