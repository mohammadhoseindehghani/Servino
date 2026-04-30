using app.Application.Common;
using app.Application.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Admins.Queries;

public record GetAdminProfileByUserIdQuery(int UserId)
    : IRequest<Result<AdminProfileDto>>;
