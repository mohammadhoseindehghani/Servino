using FluentValidation;

namespace app.Application.Features.Experts.Commands.UpdateExpertProfile;

public class UpdateExpertProfileCommandValidator
    : AbstractValidator<UpdateExpertProfileCommand>
{
    public UpdateExpertProfileCommandValidator()
    {
        RuleFor(x => x.Profile.UserId)
            .GreaterThan(0)
            .WithMessage("شناسه متخصص نامعتبر است.");
    }
}
