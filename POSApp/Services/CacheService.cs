namespace POSApp.Services;

using System.Collections.Concurrent;

public class CacheService
{
    private readonly ConcurrentDictionary<string, object> _cache = new();

    public void Set<T>(string key, T value)
    {
        _cache[key] = value!;
    }

    public T? Get<T>(string key)
    {
        if (_cache.TryGetValue(key, out var value))
        {
            return (T)value!;
        }

        return default;
    }

    public bool Contains(string key) => _cache.ContainsKey(key);

    public void Clear() => _cache.Clear();
}
