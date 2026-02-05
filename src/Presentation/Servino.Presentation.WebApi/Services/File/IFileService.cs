namespace Servino.Presentation.WebApi.Services.File;

public interface IFileService
{
    Task<string> Upload(IFormFile file, string folder, CancellationToken cancellationToken);
    Task DeleteFile(string? path, CancellationToken cancellationToken);
}