using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.DTOs.UserDTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Admins.Queries;

public class GetAdminProfileByUserIdQueryHandler(
    IAdminRepository adminRepository,
    ILogger<GetAdminProfileByUserIdQueryHandler> logger)
    : IRequestHandler<GetAdminProfileByUserIdQuery, Result<AdminProfileDto>>
{
    public async Task<Result<AdminProfileDto>> Handle(
        GetAdminProfileByUserIdQuery request,
        CancellationToken ct)
    {
        try
        {
            var profile = await adminRepository.GetByUserIdAsync(request.UserId, ct);

            return profile == null
                ? Result<AdminProfileDto>.Failure("اطلاعات یافت نشد.")
                : Result<AdminProfileDto>.Success(profile);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "System error in GetAdminProfileByUserIdQueryHandler | UserId: {UserId}",
                request.UserId);

            return Result<AdminProfileDto>.Failure(
                "خطای سیستمی رخ داده است. لطفاً مجدداً تلاش کنید.");
        }
    }
}
