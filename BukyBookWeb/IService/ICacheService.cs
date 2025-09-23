using System;
using System.Threading.Tasks;

namespace BukyBookWeb.Services
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null);
        //Task RemoveAsync(string key);
        Task RemoveByPrefixAsync(string prefix);
    }
}
