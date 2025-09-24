using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BukyBookWeb.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IConnectionMultiplexer _connection;
        private readonly JsonSerializerOptions _jsonOptions;

        public RedisCacheService(IDistributedCache cache, IConnectionMultiplexer connection)
        {
            _cache = cache;
            _connection = connection;

            
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = false
            };
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var cached = await _cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(cached)) return default;

            return JsonSerializer.Deserialize<T>(cached, _jsonOptions);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();

            if (absoluteExpireTime.HasValue)
                options.AbsoluteExpirationRelativeToNow = absoluteExpireTime;

            if (slidingExpireTime.HasValue)
                options.SlidingExpiration = slidingExpireTime;

           
            var serialized = JsonSerializer.Serialize(value, _jsonOptions);
            await _cache.SetStringAsync(key, serialized, options);

            
            var db = _connection.GetDatabase();
            var prefix = key.Split('_')[0]; 
            await db.SetAddAsync($"CacheKeys:{prefix}", key);
        }

        public async Task RemoveByPrefixAsync(string prefix)
        {
            var db = _connection.GetDatabase();
            var keys = await db.SetMembersAsync($"CacheKeys:{prefix}");

            foreach (var key in keys)
            {
                await _cache.RemoveAsync(key!);
                await db.SetRemoveAsync($"CacheKeys:{prefix}", key);
            }
        }
    }
}
