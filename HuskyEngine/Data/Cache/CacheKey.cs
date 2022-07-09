namespace HuskyEngine.Data.Cache;

public class CacheKey
{
    public CacheKey(string keySpace, params object[] components)
    {
        _keySpace = keySpace;
        _components = components.ToList();
    }

    private bool Equals(CacheKey other)
    {
        return _keySpace == other._keySpace
               && _components.SequenceEqual(other._components);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((CacheKey)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_keySpace,
            _components
                .Select(a => a.GetHashCode())
                .Aggregate(0, (a, b) => a ^ b));
    }

    private readonly string _keySpace;
    private readonly List<object> _components;
}