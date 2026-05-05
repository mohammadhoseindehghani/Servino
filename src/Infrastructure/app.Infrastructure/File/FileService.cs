using app.Application.Contracts.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace app.Infrastructure.File;

public class FileService(IWebHostEnvironment env) : IFileService
{
    private readonly string _webRootPath = env.WebRootPath;
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
    private const int MaxFileSize = 2 * 1024 * 1024; 

    public async Task<string> UploadAsync(IFormFile? file, string folder, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0) return string.Empty;

        if (file.Length > MaxFileSize)
            throw new Exception("حجم فایل نمی‌تواند بیشتر از 2 مگابایت باشد."); 

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_allowedExtensions.Contains(extension))
            throw new Exception("پسوند فایل مجاز نیست.");

        var uploadsFolder = Path.Combine(_webRootPath, "Files", folder);
        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);

        return $"/Files/{folder}/{uniqueFileName}";
    }

    public Task DeleteFileAsync(string? path, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(path)) return Task.CompletedTask;

        var fullPath = Path.Combine(_webRootPath, path.TrimStart('/'));
        if (System.IO.File.Exists(fullPath))
            System.IO.File.Delete(fullPath);

        return Task.CompletedTask;
    }
}