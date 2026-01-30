using Servino.Domain.Core._common;
using Servino.Domain.Core.RequestAgg.Dtos;
using Servino.Domain.Core.RequestAgg.Entity;

namespace Servino.Domain.Core.RequestAgg.Contracts.Data;

public interface IRequestRepository
{
    Task<int> CreateAsync(CreateRequestDto command, CancellationToken ct);
    Task<bool> UpdateAsync(UpdateRequestDto command, CancellationToken ct);
    Task<RequestFullDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<RequestDetailDto?> GetDetailsByIdAsync(int id, CancellationToken ct); 

    Task<List<RequestSummaryDto>> GetAllAsync(PaginationRequestDto search, int? categoryId, int? cityId, CancellationToken ct);
    Task<List<RequestSummaryDto>> GetByCustomerIdAsync(int customerId, CancellationToken ct);
    Task<List<RequestSummaryDto>> GetAvailableForExpertAsync(List<int> expertServiceIds, int cityId, CancellationToken ct);
    Task<bool> IsOwnerAsync(int requestId, int customerId, CancellationToken ct);
    Task<bool> IsAllowToCommentAsync(int requestId, CancellationToken ct);
}