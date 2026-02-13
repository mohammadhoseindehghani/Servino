namespace Servino.Infa.DataAccess.Repo.Dapper.Repositories.Queries;

public static class CityQueries
{
    public const string GetById = @"
SELECT 
    Id, 
    Title, 
    ProvinceId
FROM Cities
WHERE Id = @Id
  AND IsDeleted = 0;";

    public const string GetAllPaged = @"
SELECT
    c.Id,
    c.Title,
    c.ProvinceId,
    p.Title AS ProvinceName
FROM Cities c
INNER JOIN Provinces p ON p.Id = c.ProvinceId
WHERE c.IsDeleted = 0
  AND (@SearchKey IS NULL OR c.Title LIKE @SearchKey OR p.Title LIKE @SearchKey)
ORDER BY c.Id DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

    public const string GetByProvinceIdForDropdown = @"
SELECT 
    Id, 
    Title
FROM Cities
WHERE ProvinceId = @ProvinceId
  AND IsDeleted = 0
ORDER BY Title;";
}