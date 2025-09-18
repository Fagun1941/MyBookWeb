using System.Collections.Concurrent;

namespace BukyBookWeb.ErrorHandling
{
    public static class ErrorCache
    {
        // Thread-safe cache for storing unique error messages
        private static readonly ConcurrentDictionary<string, bool> _cache = new();

        /// <summary>
        /// Returns true if this message was already logged before.
        /// </summary>
        public static bool AlreadyLogged(string message)
        {
            return !_cache.TryAdd(message, true);
        }
    }
}
