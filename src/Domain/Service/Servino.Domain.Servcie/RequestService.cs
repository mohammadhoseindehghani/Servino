using Servino.Domain.Core._common;
using Servino.Domain.Core.RequestAgg.Contracts.Data;
using Servino.Domain.Core.RequestAgg.Contracts.Service;
using Servino.Domain.Core.RequestAgg.Dtos;
using Servino.Domain.Core.RequestAgg.Entity;

namespace Servino.Domain.Service;

public class RequestService(IRequestRepository requestRepo) : IRequestService
{
    public async Task<decimal> GetBasePriceByRequestIdAsync(int requestId, CancellationToken ct)
    {
        return await requestRepo.GetBasePriceByRequestIdAsync(requestId, ct);
    }

    public async Task<int> CreateAsync(CreateRequestDto command, CancellationToken ct)
    {
        return await requestRepo.CreateAsync(command, ct);
    }

    public async Task<bool> UpdateAsync(UpdateRequestDto command, CancellationToken ct)
    {
        return await requestRepo.UpdateAsync(command, ct);
    }

    public async Task<RequestFullDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await requestRepo.GetByIdAsync(id, ct);
    }

    public async Task<RequestDetailDto?> GetDetailsByIdAsync(int id, CancellationToken ct)
    {
        return await requestRepo.GetDetailsByIdAsync(id, ct);
    }

    public async Task<List<RequestSummaryDto>> GetAllAsync(PaginationRequestDto search, int? categoryId, int? cityId, CancellationToken ct)
    {
        return await requestRepo.GetAllAsync(search, categoryId, cityId, ct);
    }

    public async Task<List<RequestSummaryDto>> GetByCustomerIdAsync(int customerId, CancellationToken ct)
    {
        return await requestRepo.GetByCustomerIdAsync(customerId, ct);
    }

    public async Task<List<RequestSummaryDto>> GetAvailableForExpertAsync(List<int> expertServiceIds, int cityId, CancellationToken ct)
    {
        return await requestRepo.GetAvailableForExpertAsync(expertServiceIds, cityId, ct);
    }

    public async Task<List<RequestSummaryDto>> GetAvailableForExpertAsync(int expertId, List<int> expertServiceIds, int cityId, CancellationToken ct)
    {
        return await requestRepo.GetAvailableForExpertAsync(expertId, expertServiceIds, cityId, ct);
    }

    public async Task<bool> IsOwnerAsync(int requestId, int customerId, CancellationToken ct)
    {
        return await requestRepo.IsOwnerAsync(requestId, customerId, ct);
    }

    public async Task<bool> IsAllowToCommentAsync(int requestId, CancellationToken ct)
    {
        return await requestRepo.IsAllowToCommentAsync(requestId, ct);
    }

    public async Task<List<Request>> GetRequestsWithoutSuggestionAsync(CancellationToken ct)
    {
        return await requestRepo.GetRequestsWithoutSuggestionAsync(ct);
    }

    public async Task<bool> MarkNoSuggestionReminderSentAsync(int requestId, DateTime atUtc, CancellationToken ct)
    {
        return await requestRepo.MarkNoSuggestionReminderSentAsync(requestId, atUtc, ct);
    }

    public async Task<string?> GetCustomerMobileByRequestId(int requestId, CancellationToken ct)
    {
        return await requestRepo.GetCustomerMobileByRequestId(requestId, ct);
    }
}