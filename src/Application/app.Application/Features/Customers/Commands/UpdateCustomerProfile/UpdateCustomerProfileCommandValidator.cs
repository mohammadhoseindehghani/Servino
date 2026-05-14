using FluentValidation;

namespace app.Application.Features.Customers.Commands.UpdateCustomerProfile;

public class UpdateCustomerProfileCommandValidator
    : AbstractValidator<UpdateCustomerProfileCommand>
{
    public UpdateCustomerProfileCommandValidator()
    {
        RuleFor(x => x.Profile.UserId)
            .GreaterThan(0)
            .WithMessage("شناسه کاربر نامعتبر است.");

        RuleFor(x => x.Profile.FirstName)
            .NotEmpty()
            .WithMessage("نام الزامی است.");

        RuleFor(x => x.Profile.LastName)
            .NotEmpty()
            .WithMessage("نام خانوادگی الزامی است.");
    }
}
