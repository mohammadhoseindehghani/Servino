using Microsoft.EntityFrameworkCore;
using Servino.Domain.Core._common;
using Servino.Domain.Core.HomeServiceAgg.Entity;
using Servino.Domain.Core.RequestAgg.Contracts.Data;
using Servino.Domain.Core.RequestAgg.Dtos;
using Servino.Domain.Core.RequestAgg.Entity;
using Servino.Domain.Core.RequestAgg.Enum;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;

namespace Servino.Infa.DataAccess.Repo.EfCore.Repositories;

public class RequestRepository(AppDbContext context) : IRequestRepository
{
    public async Task<decimal> GetBasePriceByRequestIdAsync(int requestId, CancellationToken ct)
    {
        var basePrice = await context.Requests
            .Where(s => s.Id == requestId)
            .Select(s => s.HomeService.BasePrice)
            .SingleOrDefaultAsync(ct);

        if (basePrice == 0)
            throw new InvalidOperationException("Suggestion not found.");

        return basePrice;
    }


    public async Task<int> CreateAsync(CreateRequestDto dto, CancellationToken ct)
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
            CreatedAt = DateTime.UtcNow,
            Images = dto.ImagePaths?
                .Select(p => new RequestImage { ImagePath = p })
                .ToList()
        };

        context.Requests.Add(request);
        await context.SaveChangesAsync(ct);

        return request.Id;
    }


    public async Task<bool> UpdateAsync(UpdateRequestDto command, CancellationToken ct)
    {
        var affectedRows = await context.Requests
            .Where(r => r.Id == command.Id && !r.IsDeleted)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(r => r.Title, command.Title)
                    .SetProperty(r => r.Description, command.Description)
                    .SetProperty(r => r.Address, command.Address)
                    .SetProperty(r => r.CityId, command.CityId)
                    .SetProperty(r => r.DateRequired, command.DateRequired)
                    .SetProperty(r => r.Status, command.Status)
                    .SetProperty(r => r.WinnerSuggestionId, command.WinnerSuggestionId)
                    .SetProperty(r => r.DateDone, command.DateDone)
                    .SetProperty(r => r.UpdatedAt, DateTime.UtcNow),
                ct);
        return affectedRows > 0;
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
                CustomerUserId = r.Customer.UserId,
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
                CustomerId = r.CustomerId,
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

    public async Task<List<RequestSummaryDto>> GetAvailableForExpertAsync(int expertId, List<int> expertServiceIds, int cityId, CancellationToken ct)
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
                SuggestionCount = r.Suggestions.Count,
                HasExpertSuggestion = r.Suggestions.Any(s => s.ExpertId == expertId),
                ExpertSuggestionId = r.Suggestions
                    .Where(s => s.ExpertId == expertId)
                    .Select(s => (int?)s.Id)
                    .FirstOrDefault()
            })
            .ToListAsync(ct);
    }


    public async Task<bool> IsOwnerAsync(int requestId, int customerId, CancellationToken ct)
    {
        return await context.Requests
            .AnyAsync(r => r.Id == requestId && r.CustomerId == customerId, ct);
    }

    public async Task<bool> IsAllowToCommentAsync(int requestId, CancellationToken ct)
    {
        var status = await context.Requests
            .Where(r => r.Id == requestId)
            .Select(r => r.Status)
            .FirstOrDefaultAsync(ct);

        return status is RequestStatus.Done or RequestStatus.Paid;
    }

    public async Task<List<Request>> GetRequestsWithoutSuggestionAsync(CancellationToken ct)
    {
        var threshold = DateTime.UtcNow.AddHours(-3);

        return await context.Requests
            .Where(r =>
                r.Status == RequestStatus.WaitingForExperts &&
                !r.Suggestions.Any() &&
                r.CreatedAt < threshold &&
                r.NoSuggestionReminderAt == null &&
                !r.IsDeleted)
            .ToListAsync(ct);
    }


    public async Task<bool> MarkNoSuggestionReminderSentAsync(int requestId, DateTime atUtc, CancellationToken ct)
    {
        var affected = await context.Requests
            .Where(r => r.Id == requestId && !r.IsDeleted)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.NoSuggestionReminderAt, atUtc)
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow), ct);

        return affected > 0;
    }

    public async Task<string?> GetCustomerMobileByRequestId(int requestId, CancellationToken ct)
    {
        return await context.Requests
            .Where(r => r.Id == requestId)
            .Select(r => r.Customer.User.MobileNumber)
            .FirstOrDefaultAsync(ct);
    }
}
