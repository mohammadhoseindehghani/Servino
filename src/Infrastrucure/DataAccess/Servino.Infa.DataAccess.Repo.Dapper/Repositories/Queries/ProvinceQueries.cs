namespace Servino.Infa.DataAccess.Repo.Dapper.Repositories.Queries;

public static class ProvinceQueries
{
    public const string GetById = @"
SELECT 
    Id, 
    Title
FROM Provinces
WHERE Id = @Id
  AND IsDeleted = 0;";

    public const string GetAllPaged = @"
SELECT 
    Id, 
    Title
FROM Provinces
WHERE IsDeleted = 0
  AND (@SearchKey IS NULL OR Title LIKE @SearchKey)
ORDER BY Id DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

    public const string GetAllForDropdown = @"
SELECT 
    Id, 
    Title
FROM Provinces
WHERE IsDeleted = 0
ORDER BY Title;";

}