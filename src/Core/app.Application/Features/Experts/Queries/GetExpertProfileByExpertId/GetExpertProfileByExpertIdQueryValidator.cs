using FluentValidation;

namespace app.Application.Features.Experts.Queries.GetExpertProfileByExpertId;

public class GetExpertProfileByExpertIdQueryValidator
    : AbstractValidator<GetExpertProfileByExpertIdQuery>
{
    public GetExpertProfileByExpertIdQueryValidator()
    {
        RuleFor(x => x.ExpertId)
            .GreaterThan(0)
            .WithMessage("شناسه متخصص نامعتبر است.");
    }
}
