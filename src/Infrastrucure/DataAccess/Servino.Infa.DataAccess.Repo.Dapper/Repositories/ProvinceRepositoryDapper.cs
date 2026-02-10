using System.Data;
using Dapper;
using Servino.Domain.Core._common;
using Servino.Domain.Core.LocationAgg.Contracts.Data;
using Servino.Domain.Core.LocationAgg.Dtos;

namespace Servino.Infa.DataAccess.Repo.Dapper.Repositories;

public class ProvinceRepositoryDapper(IDbConnection connection) : IProvinceRepository
{
    public Task<bool> CreateAsync(string title, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(int id, string title, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<ProvinceDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        string sql = "SELECT Id, Title FROM Provinces WHERE Id = @Id AND IsDeleted = 0";
        return await connection.QueryFirstOrDefaultAsync<ProvinceDto>(sql, new { Id = id });
    }

    public async Task<List<ProvinceDto>> GetAllAsync(PaginationRequestDto search, CancellationToken ct)
    {
        string sql = "SELECT Id, Title FROM Provinces WHERE IsDeleted = 0";

        if (!string.IsNullOrWhiteSpace(search.SearchKey))
        {
            sql += " AND Title LIKE @SearchKey";
        }

        var provinces = await connection.QueryAsync<ProvinceDto>(sql, new { SearchKey = "%" + search.SearchKey + "%" });

        return provinces.AsList();
    }

    public Task<int> GetCountAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<List<SelectListDto>> GetAllForDropdownAsync(CancellationToken ct)
    {
        string sql = "SELECT Id, Title FROM Provinces WHERE IsDeleted = 0";
        var provinces = await connection.QueryAsync<SelectListDto>(sql);
        return provinces.AsList();
    }
}