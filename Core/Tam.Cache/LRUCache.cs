using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;

namespace Tam.Cache
{
    // http://kufli.blogspot.com/2009/06/aspnet-least-recently-used-lru-caching.html
    // https://github.com/ironyx/dache/tree/master/Dache.CacheHost/Storage
    // http://stackoverflow.com/questions/8195320/thread-safety-for-high-performance-in-memory-cache
    // http://stackoverflow.com/questions/8868486/whats-the-difference-between-memorycache-add-and-memorycache-set
    public static class LRUCache
    {
        private static MemoryCache Cache = MemoryCache.Default;
        private static object LockObject = new object();
        //private static Dictionary<string, int> CacheConcurrentDictionary = new Dictionary<string, int>();

        private static ConcurrentDictionary<string, int> CacheConcurrentDictionary = new ConcurrentDictionary<string, int>();
        private static int CacheTimer = 180; // cache time
        private static int LimitItems = 1000; // only cache 1000 objects

        private const string MemoryCacheTimer = "MemoryCacheTimer";
        private const string MemoryCacheLimitItems = "MemoryCacheLimitItems";

        static LRUCache()
        {
            CacheTimer = GetCacheTimer();
            LimitItems = GetLimitItems();
        }

        private static int GetCacheTimer()
        {
            try
            {
                string temp = Convert.ToString(ConfigurationManager.AppSettings[MemoryCacheTimer]);
                if (string.IsNullOrEmpty(temp))
                {
                    return 180;
                }
                return Convert.ToInt32(temp);
            }
            catch
            {
                return 180;
            }
        }

        private static int GetLimitItems()
        {
            try
            {
                string temp = Convert.ToString(ConfigurationManager.AppSettings[MemoryCacheLimitItems]);
                if (string.IsNullOrEmpty(temp))
                {
                    return 1000;
                }
                return Convert.ToInt32(temp);
            }
            catch
            {
                return 1000;
            }
        }

        //public static void Set(string key, object value, int expiredInMinute)
        //{
        //    //https://msdn.microsoft.com/en-us/library/office/ee558270(v=office.14).aspx
        //    if (value != null)
        //    {
        //        if (Cache.Contains(key) == false)
        //        {
        //            lock (LockObject)
        //            {
        //                if (Cache.Contains(key) == false)
        //                {
        //                    var policy = new CacheItemPolicy();
        //                    policy.Priority = CacheItemPriority.Default;
        //                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(expiredInMinute);
        //                    Cache.Add(key, value, policy, null);
        //                    AddToCacheConcurrentDictionary(key);
        //                }
        //            }
        //        }
        //    }
        //}

        public static void Add(string key, object value, int expiredInMinute)
        {
            //https://msdn.microsoft.com/en-us/library/office/ee558270(v=office.14).aspx
            if (value != null)
            {
                if (Cache.Contains(key) == false)
                {
                    var policy = new CacheItemPolicy();
                    policy.Priority = CacheItemPriority.Default;
                    policy.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(expiredInMinute);
                    Cache.Add(key, value, policy, null);
                    AddToCacheConcurrentDictionary(key);
                }
            }
        }

        public static void Update(string key, object value, int expiredInMinute)
        {
            Remove(key);
            Add(key, value, expiredInMinute);

            // or

            //var policy = new CacheItemPolicy();
            //policy.Priority = CacheItemPriority.Default;
            //policy.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(expiredInMinute);
            //Cache.Set(key, value, policy, null);
            //AddToCacheConcurrentDictionary(key);
        }

        /// <summary>
        /// return the key from Cache Dictionary which was least accessed
        /// </summary>
        /// <returns>Key of cache item</returns>
        private static string GetLeastAccessObject()
        {
            // Ex: CacheConcurrentDictionary data
            // Key      Value
            // u1       10
            // u2       4
            // u3       8
            // --> return u2
            var item = CacheConcurrentDictionary.OrderBy(s => s.Value).First();
            return item.Key;
        }

        private static void AddToCacheConcurrentDictionary(string key)
        {
            if (Cache.GetCount() > LimitItems)
            {
                Remove(GetLeastAccessObject());
            }
            if (CacheConcurrentDictionary.ContainsKey(key) == false)
            {
                //CacheConcurrentDictionary.Add(key, 1);
                CacheConcurrentDictionary.TryAdd(key, 1);
            }
        }

        //public static void Remove(string key)
        //{
        //    if (Cache[key] != null)
        //    {
        //        lock (LockObject)
        //        {
        //            if (Cache[key] != null)
        //            {
        //                Cache.Remove(key);
        //            }
        //        }
        //    }
        //}

        public static void Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key) == false)
            {
                if (Cache[key] != null)
                {
                    Cache.Remove(key);
                }
            }
        }

        //public static bool Exists(string key)
        //{
        //    var obj = Cache[key];
        //    if (obj != null)
        //    {
        //        lock (LockObject)
        //        {
        //            obj = Cache[key];
        //            if (obj != null)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        public static bool Exists(string key)
        {
            var obj = Cache[key];
            if (obj != null)
            {
                return true;
            }
            return false;
        }

        public static int GetLRUCount(string key)
        {
            if (CacheConcurrentDictionary.ContainsKey(key))
            {
                return CacheConcurrentDictionary[key];
            }
            return -1;
        }

        //public static object Get(string key)
        //{
        //    var obj = Cache.Get(key);
        //    if (obj != null)
        //    {
        //        return obj;
        //    }

        //    lock (LockObject)
        //    {
        //        obj = Cache[key];
        //        if (obj != null)
        //        {
        //            if (CacheConcurrentDictionary.ContainsKey(key))
        //            {
        //                // increment cache hits
        //                CacheConcurrentDictionary[key] = CacheConcurrentDictionary[key] + 1;
        //            }
        //            return obj;
        //        }
        //    }
        //    return null;
        //}

        public static object Get(string key)
        {
            var obj = Cache.Get(key);
            if (obj != null)
            {
                if (CacheConcurrentDictionary.ContainsKey(key))
                {
                    // increment cache hits
                    CacheConcurrentDictionary[key] = CacheConcurrentDictionary[key] + 1;
                }
                return obj;
            }
            return null;
        }

        //public static T Get<T>(string key) where T : class
        //{
        //    T obj = Cache.Get(key) as T;
        //    if (obj != null)
        //    {
        //        return obj;
        //    }

        //    lock (LockObject)
        //    {
        //        obj = Cache.Get(key) as T;
        //        if (obj != null)
        //        {
        //            if (CacheConcurrentDictionary.ContainsKey(key))
        //            {
        //                // increment cache hits
        //                CacheConcurrentDictionary[key] = CacheConcurrentDictionary[key] + 1;
        //            }
        //            return obj;
        //        }
        //    }
        //    return null;
        //}

        public static T Get<T>(string key) where T : class
        {
            T obj = Cache.Get(key) as T;
            if (obj != null)
            {
                if (CacheConcurrentDictionary.ContainsKey(key))
                {
                    // increment cache hits
                    CacheConcurrentDictionary[key] = CacheConcurrentDictionary[key] + 1;
                }
                return obj;
            }

            return null;
        }
    }
}