using app.Application.Common;
using app.Application.Contracts.Repositories;
using app.Application.DTOs.UserDTOs;
using MediatR;

namespace app.Application.Features.Users.Queries.GetUsersList;

public class GetUsersListQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUsersListQuery, Result<List<UserSummaryDto>>>
{
    public async Task<Result<List<UserSummaryDto>>> Handle(GetUsersListQuery request, CancellationToken ct)
    {
        var projections = await userRepository.GetAllAsync(request.Search, ct);

        return projections.Select(p => new UserSummaryDto
        {
            Id = p.Id,
            FullName = $"{p.FirstName} {p.LastName}",
            Email = p.Email,
            Mobile = p.MobileNumber,
            CityName = p.CityTitle ?? "تعیین نشده",
            Balance = p.Balance,
            IsActive = p.IsActive,
            RegisterDate = p.CreatedAt,
            ImageUrl = p.ProfileImagePath,
            Role = DetermineRole(p.HasAdmin, p.HasExpert, p.HasCustomer),
            CityId = p.CityId,
            ProvinceId = p.ProvinceId
        }).ToList();
    }
    private static string DetermineRole(bool hasAdmin, bool hasExpert, bool hasCustomer)
    {
        if (hasAdmin) return "Admin";
        if (hasExpert) return "Expert";
        return hasCustomer ? "Customer" : "Unknown";
    }
}