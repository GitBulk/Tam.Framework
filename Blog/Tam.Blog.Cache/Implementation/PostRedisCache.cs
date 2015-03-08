using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tam.Blog.Cache.Interface;
using Tam.Blog.Model;
using Tam.Blog.Repository.Interface;
using Tam.Cache;
using Tam.JsonManager;
using Tam.Redis;

namespace Tam.Blog.Cache.Implementation
{
    // Redis is thread safe. We use Redis String for PostRedisCache.
    public class PostRedisCache : BaseCache<Post>, IPostCache
    {
        //IRedisServerSettings settings = new RedisServerSettings();
        //IRedisConnection connection = new SingletonRedisConnection(settings.ConnectionStringOrName);
        //var redisManager = new RedisConnectionManager(settings, connection);
        //connectionFactory.Create()
        //ISerializer serializer = new JilJsonSerializer();
        //IRedisClient redisClient = new RedisClient(redisManager, settings, serializer);

        static IRedisClient redisClient;
        //private string PostItemFormat = "TablePost_Id_{0}";
        IPostRepository postRepository;
        //private string RedisCache12PostsKey = "TablePost_12Items";
        static string SortedSetIdsKey = "RedisCacheIds";
        static PostRedisCache()
        {
            SetUpRedisCache();
        }

        public PostRedisCache(IPostRepository postRepository)
            : base(postRepository)
        {
            this.postRepository = postRepository;
        }

        public override void Add(string valueOfId, Post entity, int expiredTime = 60)
        {
            if (string.IsNullOrWhiteSpace(valueOfId))
            {
                throw new Exception("ValueOfId is null or empty.");
            }
            if (entity == null)
            {
                throw new ArgumentNullException("Entity.");
            }
            string key = string.Format(ItemKeyFormat, valueOfId);

            // Set key to hold the string value. If key already holds a value, it is overwritten (update), regardless of its type
            // http://redis.io/commands/set
            redisClient.SetObject(key, entity, TimeSpan.FromMinutes(expiredTime));
            
            double score = BitConverter.ToDouble(entity.DataRowVersion, 0);
            redisClient.SortedSetAdd(SortedSetIdsKey, valueOfId, score);
            //base.Add(valueOfId, entity, expiredTime);
        }

        public override Post AddOrGetExistingItem(object id, int expiredInMinute)
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
            Post post = redisClient.GetObject<Post>(key);
            if (post == null)
            {
                lock (LockObject)
                {
                    post = redisClient.GetObject<Post>(key);
                    if (post == null)
                    {
                        post = this.postRepository.GetById(id);
                        redisClient.SetObject<Post>(key, post, TimeSpan.FromMinutes(expiredInMinute));
                    }
                }
            }
            return post;
            //return base.AddOrGetExistingItem(id, expiredInMinute);
        }

        public override void Delete(string valueOfId)
        {
            if (string.IsNullOrWhiteSpace(valueOfId))
            {
                throw new ArgumentNullException("ValueOfId");
            }
            string key = string.Format(ItemKeyFormat, valueOfId);
            redisClient.Remove(key);
        }

        public override bool Exists(object id)
        {
            if (string.IsNullOrWhiteSpace(Convert.ToString(id)))
            {
                throw new Exception("id is null or empty.");
            }
            string key = string.Format(ItemKeyFormat, id.ToString());
            Post post = redisClient.GetObject<Post>(key);
            return (post != null);
            //return base.Exists(id);
        }

        public List<Post> Get12NewestItems()
        {
            List<Post> posts = GetNewestItems(12);
            return posts;
        }

        public override Post GetItem(object id)
        {
            if (string.IsNullOrWhiteSpace(Convert.ToString(id)))
            {
                throw new Exception("id is null or empty.");
            }
            string key = string.Format(ItemKeyFormat, id.ToString());
            Post post = redisClient.GetObject<Post>(key);
            return post;
            //return base.GetItem(id);
        }

        public override void Update(string valueOfId, Post entity, int expiredTime = 60)
        {
            // Method StringSet of Redis supports insert/update.
            Add(valueOfId, entity, expiredTime);
            //base.Update(valueOfId, entity, expiredTime);
        }

        private static void SetUpRedisCache()
        {
            IRedisServerSettings settings = new RedisServerSettings();
            IRedisConnection connection = new SingletonRedisConnection(settings.ConnectionStringOrName);
            var redisManager = new RedisConnectionManager(settings, connection);
            ISerializer serializer = new JilJsonSerializer();
            redisClient = new RedisClient(redisManager, settings, serializer);
        }


        public List<Post> GetNewestItems(int take)
        {
            Debug.Print("Redis Post GetNewestItems()");
            var posts = GetRangeItems(0, take);
            return posts;
        }

        public List<Post> GetRangeItems(int skip, int take)
        {
            if (skip < 0)
            {
                throw new Exception("Skip must be >= 0");
            }
            if (take < 1)
            {
                throw new Exception("Take must be > 0");
            }
            Debug.Print("Redis Post GetRangeItems()");
            string key = string.Format(RangItemFormat, skip, take);
            int start = skip;
            int stop = skip + take - 1;
            var listId = redisClient.SortedSetGetAllMembers(false, key, start, stop);
            List<string> keys = listId.Select(id => string.Format(ItemKeyFormat, id)).ToList();
            var posts = new List<Post>();
            posts = redisClient.StringMultiGet<Post>(keys);
            return posts;
        }


        public int CountAllPost()
        {
            int count = redisClient.SortedSetCountAllMember(SortedSetIdsKey);
            return count;
        }
    }
}
