using app.Application.Common;
using app.Application.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Customers.Commands.UpdateCustomerProfile;

public record UpdateCustomerProfileCommand(UpdateCustomerProfileDto Profile)
    : IRequest<Result<bool>>;
