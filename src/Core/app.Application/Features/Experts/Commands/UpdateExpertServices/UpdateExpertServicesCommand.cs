using app.Application.Common;
using MediatR;

namespace app.Application.Features.Experts.Commands.UpdateExpertServices;

public record UpdateExpertServicesCommand(int UserId, List<int> SelectedIds)
    : IRequest<Result<bool>>;
