using app.Application.Common;
using app.Application.Contracts.Repositories;
using MediatR;

namespace app.Application.Features.Users.Queries.IsMobileExist;

public class IsMobileExistQueryHandler(IUserRepository
    userRepository) : IRequestHandler<IsMobileExistQuery, Result<bool>>
{

    public async Task<Result<bool>> Handle(IsMobileExistQuery request, CancellationToken ct)
    {
        var exists = await userRepository.IsMobileExistAsync(request.Mobile, ct);
        return Result<bool>.Success(exists);
    }
}
