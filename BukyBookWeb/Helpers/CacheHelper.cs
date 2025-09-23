using Microsoft.Extensions.Caching.Memory;

public static class CacheHelper
{
    private static readonly HashSet<string> Keys = new();

    public static T GetOrSet<T>(IMemoryCache cache, string key, Func<T> fetchFunc, TimeSpan? absolute = null, TimeSpan? sliding = null)
    {
        if (!cache.TryGetValue(key, out T value))
        {
            value = fetchFunc();
            var options = new MemoryCacheEntryOptions();
            if (absolute.HasValue) options.SetAbsoluteExpiration(absolute.Value);
            if (sliding.HasValue) options.SetSlidingExpiration(sliding.Value);

            cache.Set(key, value, options);

            lock (Keys) Keys.Add(key);
        }

        return value;
    }

    public static void Remove(IMemoryCache cache, string prefix)
    {
        lock (Keys)
        {
            var keysToRemove = Keys.Where(k => k.StartsWith(prefix)).ToList();
            foreach (var key in keysToRemove)
            {
                cache.Remove(key);
                Keys.Remove(key);
            }
        }
    }
}
