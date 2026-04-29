using app.Application.Common;
using MediatR;

namespace app.Application.Features.Province.Commands.DeleteProvince;

public record DeleteProvinceCommand(int Id) : IRequest<Result<bool>>;
