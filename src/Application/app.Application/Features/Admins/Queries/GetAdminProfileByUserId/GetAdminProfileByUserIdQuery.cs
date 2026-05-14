using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Admins.Queries.GetAdminProfileByUserId;

public record GetAdminProfileByUserIdQuery(int UserId)
    : IRequest<Result<AdminProfileDto>>;
