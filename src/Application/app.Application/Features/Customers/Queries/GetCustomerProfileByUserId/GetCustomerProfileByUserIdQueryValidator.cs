using FluentValidation;

namespace app.Application.Features.Customers.Queries.GetCustomerProfileByUserId;

public class GetCustomerProfileByUserIdQueryValidator
    : AbstractValidator<GetCustomerProfileByUserIdQuery>
{
    public GetCustomerProfileByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("شناسه کاربر نامعتبر است.");
    }
}
