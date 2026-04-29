using Microsoft.AspNetCore.Http;

namespace app.Application.Contracts.Services;

public interface IFileService
{
    Task<string> Upload(IFormFile file, string folder, CancellationToken cancellationToken);
    Task DeleteFile(string? path, CancellationToken cancellationToken);
}