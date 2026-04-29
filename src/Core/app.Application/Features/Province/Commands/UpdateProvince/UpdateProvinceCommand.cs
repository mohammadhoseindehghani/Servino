using app.Application.Common;
using MediatR;

namespace app.Application.Features.Province.Commands.UpdateProvince;

public record UpdateProvinceCommand(int Id, string Title) : IRequest<Result<bool>>;
