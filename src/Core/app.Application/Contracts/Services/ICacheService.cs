namespace app.Application.Contracts.Services;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken ct);
    Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken ct);
    Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan ttl, CancellationToken ct);
    Task RemoveAsync(string key, CancellationToken ct);

    Task<string> GetOrCreateStampAsync(string stampKey, TimeSpan ttl, CancellationToken ct);
    Task BumpStampAsync(string stampKey, TimeSpan ttl, CancellationToken ct);
}