using FluentValidation;

namespace app.Application.Features.Requests.Commands.UpdateRequest;

public class UpdateRequestCommandValidator : AbstractValidator<UpdateRequestCommand>
{
    public UpdateRequestCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("شناسه درخواست معتبر نیست."); 
        RuleFor(x => x.Title).NotEmpty().WithMessage("عنوان نمی تواند خالی باشد."); ;
        RuleFor(x => x.Address).NotEmpty().WithMessage("آدرس نمی تواند خالی شد.");
        RuleFor(x => x.CityId).GreaterThan(0).WithMessage("شناسه شهر معتبر نیست.");
        RuleFor(x => x.DateRequired)
            .GreaterThanOrEqualTo(DateTime.Today);
    }
}
