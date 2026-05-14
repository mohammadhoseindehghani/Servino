using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using FluentValidation;
using MediatR;

namespace app.Application.Features.Customers.Commands.UpdateCustomerProfile;

public class UpdateCustomerProfileCommandHandler(ICustomerRepository customerRepository, IValidator<UpdateCustomerProfileCommand> validator)
    : IRequestHandler<UpdateCustomerProfileCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        UpdateCustomerProfileCommand request,
        CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var result = await customerRepository.UpdateProfile(request.Profile, ct);

        return !result
            ? Result<bool>.Failure("آپدیت انجام نشد")
            : Result<bool>.Success(true, "آپدیت با موفقیت انجام شد.");
    }
}
