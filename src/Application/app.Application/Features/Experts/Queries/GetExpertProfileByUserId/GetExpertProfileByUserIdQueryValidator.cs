using FluentValidation;

namespace app.Application.Features.Experts.Queries.GetExpertProfileByUserId;

public class GetExpertProfileByUserIdQueryValidator
    : AbstractValidator<GetExpertProfileByUserIdQuery>
{
    public GetExpertProfileByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("شناسه کاربر نامعتبر است.");
    }
}
