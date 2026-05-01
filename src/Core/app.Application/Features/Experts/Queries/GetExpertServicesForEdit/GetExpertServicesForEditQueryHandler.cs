using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.DTOs.UserDTOs;
using FluentValidation;
using MediatR;

namespace app.Application.Features.Experts.Queries.GetExpertServicesForEdit;

public class GetExpertServicesForEditQueryHandler(
    IExpertRepository expertRepository,
    IHomeServiceRepository homeServiceRepository,
    IExpertHomeServiceRepository expertHomeServiceRepository,
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

        var expertId = await expertRepository.GetIdByUserIdAsync(request.UserId, ct);
        if (expertId == 0)
            return Result<List<ExpertServiceItemDto>>
                .Failure("اکسپرت یافت نشد.");

        var allServices = await homeServiceRepository.GetAllActiveServicesAsync(ct);
        var selectedServiceIds =
            await expertHomeServiceRepository.GetServiceIdsByExpertIdAsync(expertId, ct);

        var mapped = allServices.Select(s => new ExpertServiceItemDto
        {
            HomeServiceId = s.Id,
            HomeServiceTitle = s.Title,
            IsSelected = selectedServiceIds.Contains(s.Id)
        }).ToList();

        return Result<List<ExpertServiceItemDto>>.Success(mapped);
    }
}
