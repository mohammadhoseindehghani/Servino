using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.DTOs.UserDTOs;
using FluentValidation;
using MediatR;

namespace app.Application.Features.Experts.Queries.GetExpertServicesByExpertId;

public class GetExpertServicesByExpertIdQueryHandler(IExpertHomeServiceRepository expertHomeServiceRepository,
    IValidator<GetExpertServicesByExpertIdQuery> validator)
    : IRequestHandler<GetExpertServicesByExpertIdQuery, List<ExpertServiceItemDto>>
{
    public async Task<List<ExpertServiceItemDto>> Handle(
        GetExpertServicesByExpertIdQuery request,
        CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        return await expertHomeServiceRepository
            .GetExpertServicesByExpertIdAsync(request.ExpertId, ct);
    }
}
