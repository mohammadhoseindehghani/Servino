using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.ExpertHomeServiceAgg;
using app.Application.Contracts.Contracts.Services.HomeServiceAgg;
using app.Application.Contracts.Contracts.Services.UserAgg;
using app.Application.Contracts.DTOs.UserDTOs;
using FluentValidation;
using MediatR;

namespace app.Application.Features.Experts.Queries.GetExpertServicesForEdit;

public class GetExpertServicesForEditQueryHandler(
    IExpertService expertService,
    IHomeServiceService homeServiceService,
    IExpertHomeServiceService expertHomeServiceService,
    IValidator<GetExpertServicesForEditQuery> validator)
    : IRequestHandler<GetExpertServicesForEditQuery, Result<List<ExpertServiceItemDto>>>
{

    public async Task<Result<List<ExpertServiceItemDto>>> Handle(
        GetExpertServicesForEditQuery request,
        CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var expertId = await expertService.GetIdByUserIdAsync(request.UserId, ct);
        if (expertId == 0)
            return Result<List<ExpertServiceItemDto>>
                .Failure("اکسپرت یافت نشد.");

        var allServices = await homeServiceService.GetAllActiveServicesAsync(ct);
        var selectedServiceIds =
            await expertHomeServiceService.GetServiceIdsByExpertIdAsync(expertId, ct);

        var mapped = allServices.Select(s => new ExpertServiceItemDto
        {
            HomeServiceId = s.Id,
            HomeServiceTitle = s.Title,
            IsSelected = selectedServiceIds.Contains(s.Id)
        }).ToList();

        return Result<List<ExpertServiceItemDto>>.Success(mapped);
    }
}
