using Microsoft.EntityFrameworkCore;
using Servino.Domain.Core._common;
using Servino.Domain.Core.RequestAgg.Contracts.Data;
using Servino.Domain.Core.RequestAgg.Dtos;
using Servino.Domain.Core.RequestAgg.Entity;
using Servino.Domain.Core.RequestAgg.Enum;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;

namespace Servino.Infa.DataAccess.Repo.EfCore.Repositories;

public class RequestRepository(AppDbContext context) : IRequestRepository
{
    public async Task<int> CreateAsync(CreateRequestDto command, CancellationToken ct)
    {
        var request = MapToEntity(command);
        await context.Requests.AddAsync(request, ct);
        await context.SaveChangesAsync(ct);

        return request.Id;
    }

    public async Task<bool> UpdateAsync(UpdateRequestDto command, CancellationToken ct)
    {
        var request = await context.Requests
            .FirstOrDefaultAsync(r => r.Id == command.Id, ct);

        if (request == null) return false;

        request.Title = command.Title;
        request.Description = command.Description;
        request.Address = command.Address;
        request.CityId = command.CityId;
        request.DateRequired = command.DateRequired;
        request.Status = command.Status;
        request.WinnerSuggestionId = command.WinnerSuggestionId;
        request.DateDone = command.DateDone;
        request.UpdatedAt = DateTime.Now;

        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<RequestFullDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await context.Requests
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new RequestFullDto
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                Address = r.Address,
                CityId = r.CityId,
                DateRequired = r.DateRequired,
                DateDone = r.DateDone,
                Status = r.Status,
                CustomerId = r.CustomerId,
                HomeServiceId = r.HomeServiceId,
                WinnerSuggestionId = r.WinnerSuggestionId,
                ImagePaths = r.Images.Select(img => img.ImagePath).ToList()
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<RequestDetailDto?> GetDetailsByIdAsync(int id, CancellationToken ct)
    {
        return await context.Requests
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new RequestDetailDto
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                Address = r.Address,
                CityName = r.City.Title,
                DateRequired = r.DateRequired,
                RequestStatus = r.Status, 
                CustomerName = r.Customer.User.FirstName + " " + r.Customer.User.LastName,
                ImagePaths = r.Images.Select(i => i.ImagePath).ToList()
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<RequestSummaryDto>> GetAllAsync(PaginationRequestDto search, int? categoryId, int? cityId, CancellationToken ct)
    {
        var query = context.Requests.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search.SearchKey))
            query = query.Where(r => r.Title.Contains(search.SearchKey) || r.Description.Contains(search.SearchKey));

        if (categoryId.HasValue)
            query = query.Where(r => r.HomeService.CategoryId == categoryId);

        if (cityId.HasValue)
            query = query.Where(r => r.CityId == cityId);

        return await query
            .OrderByDescending(r => r.CreatedAt) 
            .Skip((search.PageNumber - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(r => new RequestSummaryDto
            {
                Id = r.Id,
                Title = r.Title,
                ServiceName = r.HomeService.Title,
                CityName = r.City.Title,
                Status = r.Status,
                DateRequired = r.DateRequired,
                CreatedAt = r.CreatedAt,
                SuggestionCount = r.Suggestions.Count
            })
            .ToListAsync(ct);
    }

    public async Task<List<RequestSummaryDto>> GetByCustomerIdAsync(int customerId, CancellationToken ct)
    {
        return await context.Requests
            .AsNoTracking()
            .Where(r => r.CustomerId == customerId)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new RequestSummaryDto
            {
                Id = r.Id,
                Title = r.Title,
                ServiceName = r.HomeService.Title,
                CityName = r.City.Title,
                Status = r.Status,
                DateRequired = r.DateRequired,
                CreatedAt = r.CreatedAt,
                SuggestionCount = r.Suggestions.Count
            })
            .ToListAsync(ct);
    }

    public async Task<List<RequestSummaryDto>> GetAvailableForExpertAsync(List<int> expertServiceIds, int cityId, CancellationToken ct)
    {
        return await context.Requests
            .AsNoTracking()
            .Where(r =>
                r.Status == RequestStatus.WaitingForExperts &&
                r.CityId == cityId &&
                expertServiceIds.Contains(r.HomeServiceId)
            )
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new RequestSummaryDto
            {
                Id = r.Id,
                Title = r.Title,
                ServiceName = r.HomeService.Title,
                CityName = r.City.Title,
                Status = r.Status,
                DateRequired = r.DateRequired,
                CreatedAt = r.CreatedAt,
                SuggestionCount = r.Suggestions.Count
            })
            .ToListAsync(ct);
    }

    public async Task<bool> IsOwnerAsync(int requestId, int customerId, CancellationToken ct)
    {
        return await context.Requests
            .AnyAsync(r => r.Id == requestId && r.CustomerId == customerId, ct);
    }





















    private Request MapToEntity(CreateRequestDto dto)
    {
        var request = new Request
        {
            Title = dto.Title,
            Description = dto.Description,
            Address = dto.Address,
            CityId = dto.CityId,
            DateRequired = dto.DateRequired,
            CustomerId = dto.CustomerId,
            HomeServiceId = dto.HomeServiceId,
            Status = RequestStatus.WaitingForExperts,
            CreatedAt = DateTime.Now
        };

        if (dto.ImagePaths != null && dto.ImagePaths.Any())
        {
            request.Images = dto.ImagePaths.Select(path => new RequestImage
            {
                ImagePath = path
            }).ToList();
        }

        return request;
    }
}
