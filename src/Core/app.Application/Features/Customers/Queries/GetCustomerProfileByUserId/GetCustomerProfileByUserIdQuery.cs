using app.Application.Common;
using app.Application.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Customers.Queries.GetCustomerProfileByUserId;

public record GetCustomerProfileByUserIdQuery(int UserId)
    : IRequest<Result<CustomerProfileDto>>;
