using Microsoft.AspNetCore.Http;

namespace app.Application.Contracts.Contracts.Services;

public interface IFileService
{
    Task<string> UploadAsync(IFormFile file, string folder, CancellationToken cancellationToken);
    Task DeleteFileAsync(string? path, CancellationToken cancellationToken);
}