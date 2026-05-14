using app.Application.Contracts.Common;
using app.Application.Contracts.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Customers.Queries.GetCustomerProfileByUserId;

public record GetCustomerProfileByUserIdQuery(int UserId)
    : IRequest<Result<CustomerProfileDto>>;
