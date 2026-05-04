using app.Application.Contracts.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace app.Infrastructure.File;

public class FileService(IWebHostEnvironment env) : IFileService
{
    private readonly string _webRootPath = env.WebRootPath;

    public async Task<string> Upload(IFormFile file, string folder, CancellationToken cancellationToken)
    {
        if (file == null! || file.Length == 0) return null!;

        var uploadsFolder = Path.Combine(_webRootPath, "Files", folder);

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        return $"/Files/{folder}/{uniqueFileName}";
    }

    public async Task DeleteFile(string? path, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(path)) return;

        var relativePath = path.TrimStart('/');

        var fullPath = Path.Combine(_webRootPath, relativePath);

        if (System.IO.File.Exists(fullPath))
        {
            await Task.Run(() => System.IO.File.Delete(fullPath), cancellationToken);
        }
    }
}