using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.Contracts.Services.RequestAgg;
using MediatR;
using Microsoft.Extensions.Logging;

namespace app.Application.Features.Requests.Commands.DeleteRequest;

public class DeleteRequestCommandHandler(
    IRequestService requestService,
    ILogger<DeleteRequestCommandHandler> logger)
    : IRequestHandler<DeleteRequestCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteRequestCommand cmd, CancellationToken ct)
    {
        try
        {
            var ok = await requestService.DeleteAsync(cmd.Id, ct);

            return ok
                ? Result<bool>.Success(true, "حذف انجام شد.")
                : Result<bool>.Failure("حذف انجام نشد.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting request {Id}", cmd.Id);
            return Result<bool>.Failure("خطای سیستمی");
        }
    }
}
