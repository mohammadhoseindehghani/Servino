namespace Servino.Infa.DataAccess.Repo.Dapper.Repositories.Queries;

public static class HomeServiceQueries
{
    public const string GetAllActive = @"
SELECT 
    hs.Id,
    hs.Title,
    c.Title AS CategoryName,
    hs.BasePrice,
    hs.VisitCount,
    hs.ImagePath
FROM HomeServices hs
INNER JOIN Categories c ON hs.CategoryId = c.Id
WHERE hs.IsActive = 1 
  AND hs.IsDeleted = 0
ORDER BY hs.VisitCount DESC, hs.Id DESC;";

    public const string GetById = @"
SELECT 
    Id,
    Title,
    BasePrice,
    CategoryId,
    ShortDescription,
    ImagePath
FROM HomeServices
WHERE Id = @Id 
  AND IsDeleted = 0;";

    public const string GetAllPaged = @"
SELECT
    hs.Id,
    hs.Title,
    c.Title AS CategoryName,
    hs.BasePrice,
    hs.VisitCount,
    hs.ImagePath
FROM HomeServices hs
INNER JOIN Categories c ON hs.CategoryId = c.Id
WHERE hs.IsDeleted = 0
  AND (@SearchKey IS NULL OR hs.Title LIKE @SearchKey OR c.Title LIKE @SearchKey)
ORDER BY hs.VisitCount DESC, hs.Id DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";
}