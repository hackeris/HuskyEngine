using Microsoft.Extensions.Caching.Memory;

namespace HuskyEngine.Data.Cache;

public class DataCache
{
    private readonly int _sizeLimit;
    private readonly MemoryCache _cache;

    public DataCache()
    {
        _sizeLimit = 20000;
        _cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = _sizeLimit });
    }

    public int Used()
    {
        return _cache.Count;
    }

    public int MaxSize()
    {
        return _sizeLimit;
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