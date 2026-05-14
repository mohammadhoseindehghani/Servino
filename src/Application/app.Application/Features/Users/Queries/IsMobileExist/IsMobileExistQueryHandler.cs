using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.UserAgg;
using FluentValidation;
using MediatR;

namespace app.Application.Features.Users.Queries.IsMobileExist;

public class IsMobileExistQueryHandler(IUserService
    userService, IValidator<IsMobileExistQuery> validator) : IRequestHandler<IsMobileExistQuery, Result<bool>>
{

    public async Task<Result<bool>> Handle(IsMobileExistQuery request, CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var exists = await userService.IsMobileExistAsync(request.Mobile, ct);
        return Result<bool>.Success(exists);
    }
}
