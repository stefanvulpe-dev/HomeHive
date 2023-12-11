using System.Collections;
using System.Collections.Concurrent;
using System.Text.Json;
using HomeHive.Application.Contracts.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace HomeHive.Infrastructure.Caching;

public class CacheService(IDistributedCache distributedCache) : ICacheService
{
    private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();
    
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        var cachedValue = await distributedCache.GetStringAsync(key, cancellationToken);
        return cachedValue is null ? null : JsonSerializer.Deserialize<T>(cachedValue);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? slidingExpirationTime = null,
        CancellationToken cancellationToken = default) where T : class
    {
        if (slidingExpirationTime != null)
            await distributedCache.SetStringAsync(key, JsonSerializer.Serialize(value), new DistributedCacheEntryOptions
            {
                SlidingExpiration = slidingExpirationTime
            }, cancellationToken);
        else
            await distributedCache.SetStringAsync(key, JsonSerializer.Serialize(value), cancellationToken);
        CacheKeys.TryAdd(key, false);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await distributedCache.RemoveAsync(key, cancellationToken);
        CacheKeys.TryRemove(key, out _);
    }

    public async Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
    {
        var tasks = CacheKeys.Keys.Where(key => key.Contains(pattern))
            .Select(k => RemoveAsync(k, cancellationToken));
        await Task.WhenAll(tasks);
    }
}