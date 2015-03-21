using Enyim.Caching;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Cache.Memcached
{
    public class MemcachedProvider : ICachingProvider
    {
        // http://gokberkhamurcu.blogspot.com/2012/07/caching-objects-in-memcached-using-c.html
        // http://stackoverflow.com/questions/6164442/asp-net-mvc-session-state-using-state-partitioning-mongodb-or-memcached-or
        static MemcachedClient MemcachedClient;

        static MemcachedProvider()
        {
            MemcachedClient = new MemcachedClientFactory().Instance();
        }

        public MemcachedProvider()
        {
            //this.memcachedClient = new MemcachedClient();
        }

        public void Set<T>(string key, T value, TimeSpan ts, CacheItemPriority priority = CacheItemPriority.Default)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            if (ts == null)
            {
                ts = TimeSpan.FromMinutes(10);
            }
            MemcachedClient.Store(StoreMode.Set, key, value, ts);
        }

        public void Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            MemcachedClient.Remove(key);
        }

        public bool Exists(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            return (MemcachedClient.Get(key) != null);
        }

        public bool Get<T>(string key, out T value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            try
            {
                value = MemcachedClient.Get<T>(key);
                if (Equals(value, default(T)))
                {
                    value = default(T);
                    return false;
                }
            }
            catch
            {
                value = default(T);
                return false;
            }
            return true;
        }

        public long Increment(string key, int defaultValue, int incrementValue, CacheItemPriority priority = CacheItemPriority.Default)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            return Convert.ToInt64(MemcachedClient.Increment(key, Convert.ToUInt64(defaultValue), Convert.ToUInt64(incrementValue)));
        }

        public void Dispose()
        {
            MemcachedClient.Dispose();
        }
    }
}
