using app.Application.Contracts.Common;
using MediatR;

namespace app.Application.Features.Users.Queries.IsMobileExist;

public record IsMobileExistQuery(string Mobile)
    : IRequest<Result<bool>>;
