using FluentValidation;

namespace app.Application.Features.Admins.Queries;

public class GetAdminProfileByUserIdQueryValidator
    : AbstractValidator<GetAdminProfileByUserIdQuery>
{
    public GetAdminProfileByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("شناسه کاربر نامعتبر است.");
    }
}
