using System;
using System.Runtime.Caching;
using Tam.Redis;

namespace Tam.Cache.Redis
{
    public class RedisCacheProvider : ICachingProvider
    {
        IRedisClient redisClient;

        public RedisCacheProvider(IRedisClient redisClient)
        {
            this.redisClient = redisClient;
        }

        public void Set<T>(string key, T value, TimeSpan ts, System.Runtime.Caching.CacheItemPriority priority = CacheItemPriority.Default)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            if (ts == null)
            {
                ts = TimeSpan.FromMinutes(10);
            }
            this.redisClient.SetObject<T>(key, value, ts);
        }

        public void Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            this.redisClient.Remove(key);
        }

        public bool Exists(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            return this.redisClient.KeyExists(key);
        }

        public bool Get<T>(string key, out T value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            try
            {
                value = this.redisClient.GetObject<T>(key);
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

        public long Increment(string key, int defaultValue, int incrementValue, System.Runtime.Caching.CacheItemPriority priority = CacheItemPriority.Default)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            return this.redisClient.StringIncrement(key, incrementValue);
        }

        public void Dispose()
        {
        }
    }
}
