using app.Application.DTOs.LocationDTOs;
using MediatR;

namespace app.Application.Features.Province.Queries.GetProvincesForDropdown;

public record GetProvincesForDropdownQuery()
    : IRequest<List<SelectListDto>>;
