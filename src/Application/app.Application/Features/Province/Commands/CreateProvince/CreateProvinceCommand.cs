using app.Application.Contracts.Common;
using MediatR;

namespace app.Application.Features.Province.Commands.CreateProvince;

public record CreateProvinceCommand(string Title) : IRequest<Result<bool>>;
