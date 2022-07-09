using Microsoft.Extensions.Caching.Memory;

namespace HuskyEngine.Data.Cache;

public class DataCache
{
    private readonly MemoryCache _cache;

    public DataCache()
    {
        _cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 20000 });
    }

    public T GetOrLoad<T>(
        CacheKey key,
        Func<T> loader
    )
    {
        return _cache.GetOrCreate(key, entry =>
        {
            entry.Size = 1;
            return loader.Invoke();
        });
    }
}