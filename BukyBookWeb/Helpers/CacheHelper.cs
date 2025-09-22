using Microsoft.Extensions.Caching.Memory;

namespace BukyBookWeb.Helpers
{
    public static class CacheHelper
    {
        public static T GetOrSet<T>(IMemoryCache cache, string key, Func<T> fetchFunc, TimeSpan? absolute = null, TimeSpan? sliding = null)
        {
            if (!cache.TryGetValue(key, out T value))
            {
                value = fetchFunc();
                var options = new MemoryCacheEntryOptions();
                if (absolute.HasValue) options.SetAbsoluteExpiration(absolute.Value);
                if (sliding.HasValue) options.SetSlidingExpiration(sliding.Value);

                cache.Set(key, value, options);
            }

            return value;
        }

        public static void Remove(IMemoryCache cache, string key)
        {
            cache.Remove(key);
        }
    }
}
