using app.Application.Common;
using app.Application.Contracts.Repositories;
using MediatR;

namespace app.Application.Features.Customers.Commands.UpdateCustomerProfile;

public class UpdateCustomerProfileCommandHandler(ICustomerRepository customerRepository)
    : IRequestHandler<UpdateCustomerProfileCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        UpdateCustomerProfileCommand request,
        CancellationToken ct)
    {
        var result = await customerRepository.UpdateProfile(request.Profile, ct);

        return !result
            ? Result<bool>.Failure("آپدیت انجام نشد")
            : Result<bool>.Success(true, "آپدیت با موفقیت انجام شد.");
    }
}
