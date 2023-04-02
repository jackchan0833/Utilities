using JC.Utilities.Constants;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.Utilities.Cache
{
    /// <summary>
    /// Represents the <see cref="CacheManager"/>.
    /// </summary>
    public partial class CacheManager
    {
        private static ConcurrentDictionary<string, object> CacheValues = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// Adds the cache with specified key.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="cacheValue">The value to cache.</param>
        public static void AddOrUpdate(string key, object cacheValue)
        {
            CacheValues.AddOrUpdate(key, cacheValue, (a,b) => { return cacheValue; });
        }
        /// <summary>
        /// Try to get the cache value with specified key.
        /// </summary>
        /// <typeparam name="TData">The type of cache value.</typeparam>
        /// <param name="key">The specified cache key.</param>
        /// <param name="data">The output cache value.</param>
        /// <returns>True if the key was found; otherwise, false.</returns>
        public static bool TryGet<TData>(string key, out TData data)
        {
            if (CacheValues.TryGetValue(key, out object objValue))
            {
                data = (TData)objValue;
                return true;
            }
            data = default(TData);
            return false;
        }
        /// <summary>
        /// Remove the cache with specified key.
        /// </summary>
        /// <param name="key">The specified cache key to remove.</param>
        /// <returns>True if the cache object was removed successfully; otherwise, false.</returns>
        public static bool RemoveCache(string key)
        {
            return CacheValues.TryRemove(key, out object val);
        }

        /// <summary>
        /// Clear all cache objects.
        /// </summary>
        public static void ClearAll()
        {
            CacheValues.Clear();
        }
    }
}
