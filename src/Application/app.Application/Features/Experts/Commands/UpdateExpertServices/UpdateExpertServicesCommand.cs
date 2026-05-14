using app.Application.Contracts.Common;
using MediatR;

namespace app.Application.Features.Experts.Commands.UpdateExpertServices;

public record UpdateExpertServicesCommand(int UserId, List<int> SelectedIds)
    : IRequest<Result<bool>>;
