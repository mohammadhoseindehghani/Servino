using Microsoft.AspNetCore.Http;

namespace app.Application.Contracts.Contracts.Providers_Services;

public interface IFileService
{
    Task<string> UploadAsync(IFormFile file, string folder, CancellationToken cancellationToken);
    Task DeleteFileAsync(string? path, CancellationToken cancellationToken);
}