using FluentValidation;

namespace app.Application.Features.Requests.Commands.CreateRequest;

public class CreateRequestCommandValidator : AbstractValidator<CreateRequestCommand>
{
    public CreateRequestCommandValidator()
    {
        RuleFor(x => x.DateRequired)
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("تاریخ درخواست نمی‌تواند در گذشته باشد.");

        RuleFor(x => x.CityId)
            .GreaterThan(0)
            .WithMessage("انتخاب شهر الزامی است.");

        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("مشتری معتبر نیست.");

        RuleFor(x => x.HomeServiceId)
            .GreaterThan(0)
            .WithMessage("انتخاب خدمات الزامی است.");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("آدرس نمی‌تواند خالی باشد.");

        RuleFor(x => x.ImagePaths)
            .Must(x => x == null || x.Count <= 5)
            .WithMessage("حداکثر ۵ تصویر می‌توانید آپلود کنید.");
    }
}
