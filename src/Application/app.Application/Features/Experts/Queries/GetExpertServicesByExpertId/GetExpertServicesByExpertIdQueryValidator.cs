using FluentValidation;

namespace app.Application.Features.Experts.Queries.GetExpertServicesByExpertId;

public class GetExpertServicesByExpertIdQueryValidator
    : AbstractValidator<GetExpertServicesByExpertIdQuery>
{
    public GetExpertServicesByExpertIdQueryValidator()
    {
        RuleFor(x => x.ExpertId)
            .GreaterThan(0)
            .WithMessage("شناسه متخصص نامعتبر است.");
    }
}
