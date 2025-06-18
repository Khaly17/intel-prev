using System;
using System.Collections.Generic;

namespace Soditech.IntelPrev.Web.Services.Cache;

public class CacheService : ICacheService
{
    private readonly Dictionary<string, (DateTimeOffset, object)> _cache = new();
    

    public void Set(string key, object value)
    {
        _cache[key] = (DateTimeOffset.UtcNow, value);
    }

    public (bool Exists, object Value) Get(string key)
    {
        if (!_cache.TryGetValue(key, out var cacheEntry))
            return (false, null)!;
        // Cache for 5 minutes
        if (DateTimeOffset.UtcNow - cacheEntry.Item1 < TimeSpan.FromMinutes(5))
        {
            return (true, cacheEntry.Item2);
        }
        
        // Remove cache if expired
        _cache.Remove(key); 
        return (false, null)!;
    }
}
