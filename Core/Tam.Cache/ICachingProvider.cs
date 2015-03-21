using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Cache
{
    public interface ICachingProvider : IDisposable
    {
        /// <summary>
        /// Create an item in cache engine. Item is key/value pair
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Key of item</param>
        /// <param name="value">Value of item</param>
        /// <param name="ts">Expiry time</param>
        /// <param name="priority">priority</param>
        void Set<T>(string key, T value, TimeSpan ts, CacheItemPriority priority = CacheItemPriority.Default);

        /// <summary>
        /// Remove item from cache engine
        /// </summary>
        /// <param name="key">Key of item</param>
        void Remove(string key);

        /// <summary>
        /// Check for item in cache engine
        /// </summary>
        /// <param name="key">Key of item</param>
        /// <returns></returns>
        bool Exists(string key);

        /// <summary>
        /// Get cache item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Key of item</param>
        /// <param name="value">Cached value. default(T) if item doesn't exist</param>
        /// <returns>True: get item sucessfully</returns>
        bool Get<T>(string key, out T value);

        /// <summary>
        /// Increase value of cache item
        /// </summary>
        /// <param name="key">Key of item</param>
        /// <param name="defaultValue"></param>
        /// <param name="incrementValue">IncrementValue</param>
        /// <param name="priority"></param>
        /// <returns></returns>
        long Increment(string key, int defaultValue, int incrementValue, CacheItemPriority priority = CacheItemPriority.Default);
    }
}
