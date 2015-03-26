using NLog;
using System;
using Tam.Blog.Cache.Interface;
using Tam.Cache;
using Tam.Repository.Contraction;

namespace Tam.Blog.Cache.Implementation
{
    public abstract class BaseCache<T> : IBaseCache<T> where T : class
    {
        protected static readonly object LockObject = new object();

        /// <summary>
        /// CacheFormat string of one item (row) of table XXX
        /// ex: CacheItem_Table" + typeof(T).Name + "_Id_{0}
        /// </summary>
        protected readonly string ItemKeyFormat = "CacheItem_Table" + typeof(T).Name + "_Id_{0}";

        /// <summary>
        /// "Table" + typeof(T).Name + "_Skip_{0})_Take_{1}";
        /// </summary>
        protected readonly string RangItemFormat = "Table" + typeof(T).Name + "_Skip_{0})_Take_{1}";
        private IBaseRepository<T> baseRepository;

        public BaseCache(IBaseRepository<T> baseRepository)
        {
            this.baseRepository = baseRepository;
        }

        protected BaseCache()
        {
            NLog = LogManager.GetLogger(GetType().FullName);
        }

        protected Logger NLog { get; private set; }
        public virtual void Add(string valueOfId, T entity, int expiredTime = 60) // 60 minutes
        {
            if (string.IsNullOrWhiteSpace(valueOfId))
            {
                throw new ArgumentNullException("ValueOfId");
            }
            if (entity == null)
            {
                throw new ArgumentNullException("Entity");
            }
            string key = string.Format(ItemKeyFormat, valueOfId);
            LRUCache.Add(key, entity, expiredTime);
        }

        public virtual T AddOrGetExistingItem(object id, int expiredInMinute)
        {
            if (string.IsNullOrWhiteSpace(Convert.ToString(id)))
            {
                throw new Exception("id is null or empty.");
            }
            if (expiredInMinute < 0)
            {
                throw new Exception("ExpiredInMinute must be > 0.");
            }
            string key = string.Format(ItemKeyFormat, id.ToString());
            T item = LRUCache.Get<T>(key);
            if (item == null)
            {
                lock (LockObject)
                {
                    item = LRUCache.Get<T>(key);
                    if (item == null)
                    {
                        item = this.baseRepository.GetById(id);
                        LRUCache.Add(key, item, expiredInMinute);
                    }
                }
            }
            return item;
        }

        public virtual void Delete(string valueOfId)
        {
            if (string.IsNullOrWhiteSpace(valueOfId))
            {
                throw new ArgumentNullException("ValueOfId");
            }
            string key = string.Format(ItemKeyFormat, valueOfId);
            LRUCache.Remove(key);
        }

        public virtual bool Exists(object id)
        {
            if (string.IsNullOrWhiteSpace(Convert.ToString(id)))
            {
                throw new Exception("id is null or empty.");
            }
            string key = string.Format(ItemKeyFormat, id.ToString());
            //object item = LRUCache.Get(key);
            //return (item != null);
            bool result = LRUCache.Exists(key);
            return result;
        }


        public virtual T GetItem(object id)
        {
            if (string.IsNullOrWhiteSpace(Convert.ToString(id)))
            {
                throw new Exception("id is null or empty.");
            }
            string key = string.Format(ItemKeyFormat, id.ToString());
            T item = LRUCache.Get<T>(key);
            return item;
        }

        public virtual void Update(string valueOfId, T entity, int expiredTime = 60)
        {
            if (string.IsNullOrWhiteSpace(valueOfId))
            {
                throw new ArgumentNullException("ValueOfId");
            }
            if (entity == null)
            {
                throw new ArgumentNullException("Entity");
            }
            string key = string.Format(ItemKeyFormat, valueOfId);
            LRUCache.Update(key, entity, expiredTime);
        }
    }
}
