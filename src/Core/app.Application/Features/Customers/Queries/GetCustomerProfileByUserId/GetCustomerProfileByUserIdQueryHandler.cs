using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.DTOs.UserDTOs;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Customers.Queries.GetCustomerProfileByUserId;

public class GetCustomerProfileByUserIdQueryHandler(
    ICustomerRepository customerRepository,
    ILogger<GetCustomerProfileByUserIdQueryHandler> logger,
    IValidator<GetCustomerProfileByUserIdQuery> validator)
    : IRequestHandler<GetCustomerProfileByUserIdQuery, Result<CustomerProfileDto>>
{
    public async Task<Result<CustomerProfileDto>> Handle(
        GetCustomerProfileByUserIdQuery request,
        CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        try
        {
            var profile = await customerRepository.GetByUserIdAsync(request.UserId, ct);

            return profile == null
                ? Result<CustomerProfileDto>.Failure("اطلاعات یافت نشد.")
                : Result<CustomerProfileDto>.Success(profile);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "System error in GetCustomerProfileByUserIdQueryHandler | UserId: {UserId}",
                request.UserId);

            return Result<CustomerProfileDto>.Failure(
                "خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }
}
