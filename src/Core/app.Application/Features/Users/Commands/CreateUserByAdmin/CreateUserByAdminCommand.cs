using app.Application.Common;
using app.Application.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Users.Commands.CreateUserByAdmin;

public record CreateUserByAdminCommand(CreateUserByAdminDto Command)
    : IRequest<Result<bool>>;
