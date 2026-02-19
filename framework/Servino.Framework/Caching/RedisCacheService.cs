using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Servino.Framework.Caching;

public class RedisCacheService(IDistributedCache cache) : ICacheService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct)
    {
        var data = await cache.GetStringAsync(key, ct);
        if (string.IsNullOrWhiteSpace(data)) return default;
        return JsonSerializer.Deserialize<T>(data, JsonOptions);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken ct)
    {
        var json = JsonSerializer.Serialize(value, JsonOptions);
        await cache.SetStringAsync(key, json, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl
        }, ct);
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan ttl, CancellationToken ct)
    {
        var cached = await GetAsync<T>(key, ct);
        if (cached is not null) return cached;

        var value = await factory();
        await SetAsync(key, value, ttl, ct);
        return value;
    }

    public Task RemoveAsync(string key, CancellationToken ct)
        => cache.RemoveAsync(key, ct);


    public async Task<string> GetOrCreateStampAsync(string stampKey, TimeSpan ttl, CancellationToken ct)
    {
        var stamp = await cache.GetStringAsync(stampKey, ct);
        if (!string.IsNullOrWhiteSpace(stamp)) return stamp;

        stamp = Guid.NewGuid().ToString("N");
        await cache.SetStringAsync(stampKey, stamp, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl
        }, ct);

        return stamp;
    }

    public async Task BumpStampAsync(string stampKey, TimeSpan ttl, CancellationToken ct)
    {
        var stamp = Guid.NewGuid().ToString("N");
        await cache.SetStringAsync(stampKey, stamp, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl
        }, ct);
    }

}