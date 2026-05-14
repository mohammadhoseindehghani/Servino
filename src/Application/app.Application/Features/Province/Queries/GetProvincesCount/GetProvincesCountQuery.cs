using MediatR;

namespace app.Application.Features.Province.Queries.GetProvincesCount;

public record GetProvincesCountQuery() : IRequest<int>;
