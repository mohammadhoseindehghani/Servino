namespace Servino.Infa.DataAccess.Repo.Dapper.Repositories.Queries;

public static class CategoryQueries
{
    public const string GetById = @"
SELECT 
    Id, 
    Title, 
    ParentId, 
    ImagePath
FROM Categories
WHERE Id = @Id 
  AND IsDeleted = 0;";

    public const string GetAllPaged = @"
SELECT
    c.Id,
    c.Title,
    p.Title AS ParentTitle,
    (
        SELECT COUNT(1)
        FROM Categories sc
        WHERE sc.ParentId = c.Id
          AND sc.IsDeleted = 0
    ) AS SubCategoriesCount,
    c.IsActive,
    c.ImagePath
FROM Categories c
LEFT JOIN Categories p ON p.Id = c.ParentId
WHERE c.IsDeleted = 0
  AND (@SearchKey IS NULL OR c.Title LIKE @SearchKey OR p.Title LIKE @SearchKey)
ORDER BY c.CreatedAt DESC, c.Id DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

    public const string GetByParentId = @"
SELECT 
    Id, 
    Title, 
    ImagePath
FROM Categories
WHERE ParentId = @ParentId
  AND IsDeleted = 0 
  AND IsActive = 1
ORDER BY Title;";

    public const string GetServicesByCategoryId = @"
SELECT 
    Id, 
    Title, 
    BasePrice, 
    ShortDescription, 
    ImagePath
FROM HomeServices
WHERE CategoryId = @CategoryId
  AND IsDeleted = 0
ORDER BY VisitCount DESC, Id DESC;";
}