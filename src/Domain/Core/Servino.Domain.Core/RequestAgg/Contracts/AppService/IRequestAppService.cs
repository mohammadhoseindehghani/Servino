using Servino.Domain.Core._common;
using Servino.Domain.Core.RequestAgg.Dtos;

namespace Servino.Domain.Core.RequestAgg.Contracts.AppService;

public interface IRequestAppService
{
    Task<Result<int>> CreateAsync(CreateRequestDto command, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(UpdateRequestDto command, CancellationToken ct);
    Task<Result<RequestFullDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<RequestDetailDto>> GetDetailsByIdAsync(int id, CancellationToken ct);
    Task<List<RequestSummaryDto>> GetAllAsync(PaginationRequestDto search, int? categoryId, int? cityId, CancellationToken ct);
    Task<List<RequestSummaryDto>> GetByCustomerIdAsync(int customerId, CancellationToken ct);
    Task<List<RequestSummaryDto>> GetAvailableForExpertAsync(int expertId, CancellationToken ct);
    Task<Result<bool>> CancelRequestAsync(int requestId, int customerId, CancellationToken ct);
    Task<Result<bool>> MarkAsDoneAndPayAsync(int requestId, int customerId, CancellationToken ct);
    Task<Result<bool>> SelectExpertAsync(int requestId, int suggestionId, int customerId, CancellationToken ct);
}