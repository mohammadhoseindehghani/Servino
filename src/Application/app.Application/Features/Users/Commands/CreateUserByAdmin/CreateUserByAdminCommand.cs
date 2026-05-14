using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Users.Commands.CreateUserByAdmin;

public record CreateUserByAdminCommand(CreateUserByAdminDto Command)
    : IRequest<Result<bool>>;
