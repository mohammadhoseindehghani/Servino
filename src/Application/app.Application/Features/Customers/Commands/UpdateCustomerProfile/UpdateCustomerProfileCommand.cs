using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Customers.Commands.UpdateCustomerProfile;

public record UpdateCustomerProfileCommand(UpdateCustomerProfileDto Profile)
    : IRequest<Result<bool>>;
