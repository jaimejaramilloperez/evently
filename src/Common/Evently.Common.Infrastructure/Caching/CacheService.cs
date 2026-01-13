using System.Text.Json;
using Evently.Common.Application.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace Evently.Common.Infrastructure.Caching;

public sealed class CacheService(IDistributedCache cache)
    : ICacheService
{
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        byte[]? data = await cache.GetAsync(key, cancellationToken);

        return data is null ? default : JsonSerializer.Deserialize<T>(data);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        byte[] data = JsonSerializer.SerializeToUtf8Bytes(value);

        await cache.SetAsync(key, data, CacheOptions.Create(expiration), cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await cache.RemoveAsync(key, cancellationToken);
    }
}
