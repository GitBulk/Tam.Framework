using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tam.JsonManager;

namespace Tam.Redis
{
    public class RedisClient : IRedisClient
    {
        private readonly IDatabase database;
        private readonly CommandFlags commandFlag;
        private ISerializer serializer;

        public RedisClient(IRedisConnectionManager connectionManager,
            IRedisServerSettings settings, ISerializer serializer)
        {
            this.database = connectionManager.Database(settings.DefaultDb);
            this.serializer = serializer;
            this.commandFlag = settings.PreferSlaveForRead ? CommandFlags.PreferSlave : CommandFlags.PreferMaster;
        }

        public RedisClient(IRedisConnectionManager connectionManager,
            IRedisServerSettings settings)
            : this(connectionManager, settings, new NewtonSoftJsonSerializer())
        {
        }

        public bool KeyExists(string key)
        {
            return this.database.KeyExists(key, this.commandFlag);
        }

        public T GetObject<T>(string key)
        {
            return this.serializer.Deserialize<T>(this.database.StringGet(key, this.commandFlag));
        }

        public void SetObject<T>(string key, T value, TimeSpan expiredIn)
        {
            SetObject(key, value, expiredIn, CommandFlags.FireAndForget);
        }

        public void SetObject<T>(string key, T value, TimeSpan expiredIn, CommandFlags commandFlag)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new Exception("Key is empty or null");
            }
            if (value == null)
            {
                throw new ArgumentNullException("Value");
            }
            // usesage of expiredId: TimeSpan.FromHours(1) --> cache life time is 1 hour
            string redisValue = this.serializer.Serialize(value);
            this.database.StringSet(key, redisValue, expiredIn, When.Always, commandFlag);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await this.database.KeyExistsAsync(key, this.commandFlag);
        }

        public async Task<T> GetObjectAsync<T>(string key)
        {
            var reault = await this.database.StringGetAsync(key, this.commandFlag);
            return this.serializer.Deserialize<T>(reault);
        }

        public async Task SetObjectAsync<T>(string key, T value, TimeSpan expiredIn)
        {
            await SetObjectAsync<T>(key, value, expiredIn, CommandFlags.FireAndForget);
        }

        public async Task SetObjectAsync<T>(string key, T value, TimeSpan expiredIn, CommandFlags commandFlags)
        {
            await this.database.StringSetAsync(key, this.serializer.Serialize(value), expiredIn, When.Always, commandFlag);
        }

        public void Remove(string key, CommandFlags commandFlag)
        {
            this.database.KeyDelete(key, commandFlag);
        }

        public async Task RemoveAsync(string key, CommandFlags commandFlags = CommandFlags.None)
        {
            await this.database.KeyDeleteAsync(key, commandFlag);
        }


        public async Task<T> GetObjectAsync<T>(string key, TimeSpan expiredIn, Func<T> fetch)
        {
            if (KeyExists(key))
            {
                return await GetObjectAsync<T>(key);
            }
            else
            {
                var result = fetch();

                if (result != null)
                {
                    await SetObjectAsync<T>(key, result, expiredIn);
                }

                return result;
            }
        }

        public T GetObject<T>(string key, TimeSpan expiredIn, Func<T> fetch)
        {
            if (KeyExists(key))
            {
                return GetObject<T>(key);
            }
            else
            {
                var result = fetch();

                if (result != null)
                {
                    SetObject(key, result, expiredIn);
                }

                return result;
            }
        }

        public void StringSet(string key, string value)
        {
            this.database.StringSet(key, value, null, When.Always, CommandFlags.FireAndForget);
        }

        public long StringIncrement(string key, long value)
        {
            var result = this.database.StringIncrement(key, value);
            return result;
        }

        public double StringIncrement(string key, double value)
        {
            return this.database.StringIncrement(key, value);
        }


        public dynamic Get(string key)
        {
            RedisValue value = this.database.StringGet(key);
            return value;
        }


        public string StringGet(string key)
        {
            return this.database.StringGet(key);
        }

        public void SortedSetAdd(string key, string memberName, double score)
        {
            this.database.SortedSetAdd(key, memberName, score);
        }


        public void SortedSetIncrement(string key, string memberName, double score)
        {
            this.database.SortedSetIncrement(key, memberName, score);
        }

        public void SortedSetIncrementOne(string key, string memberName)
        {
            SortedSetIncrement(key, memberName, 1);
        }

        public double? SortedSetGetScore(string key, string memberName)
        {
            return this.database.SortedSetScore(key, memberName);
        }

        public Task<double?> SortedSetGetScoreAsync(string key, string memberName)
        {
            return this.database.SortedSetScoreAsync(key, memberName);
        }


        public IDictionary<string, double> SortedSetGetAllMembersWithScore(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            //RedisValue[] values = this.database.SortedSetRangeByScoreWithScores(key, order: Order.Descending);
            //http://redis.io/commands/zrangebyscore
            SortedSetEntry[] entries = this.database.SortedSetRangeByScoreWithScores(key, order: Order.Descending);
            Dictionary<string, double> members = null;
            if (entries != null && entries.Any())
            {
                members = new Dictionary<string, double>();
                foreach (var item in entries)
                {
                    members.Add(item.ToString(), item.Score);
                }
            }
            return members;
        }

        public List<string> StringMultiGet(params string[] arrayKey)
        {
            if (arrayKey == null || arrayKey.Length == 0)
            {
                return null;
            }
            List<string> result = null;
            int length = arrayKey.Length;
            var keys = new RedisKey[length];
            for (int i = 0; i < length; i++)
            {
                keys[i] = arrayKey[i];
            }
            RedisValue[] items = this.database.StringGet(keys);
            if (items != null && items.Any())
            {
                result = new List<string>();
                foreach (var item in items)
                {
                    result.Add(item.ToString());
                }
            }
            return result;
        }


        public IDictionary<string, string> StringMultiGetWithKey(params string[] arrayKey)
        {
            if (arrayKey == null || arrayKey.Length == 0)
            {
                return null;
            }
            Dictionary<string, string> result = null;
            int length = arrayKey.Length;
            var keys = new RedisKey[length];
            for (int i = 0; i < length; i++)
            {
                keys[i] = arrayKey[i];
            }
            RedisValue[] items = this.database.StringGet(keys);
            if (items != null && items.Any())
            {
                result = new Dictionary<string, string>();
                int index = 0;
                foreach (var item in items)
                {
                    result.Add(arrayKey[index], item.ToString());
                    index++;
                }
            }
            return result;
        }


        public List<string> SortedSetGetAllMembers(bool ascending, string key, int start = 0, int stop = -1)
        {
            if (key == null)
            {
                return null;
            }
            Order order = (ascending ? Order.Ascending : Order.Descending);
            RedisValue[] items = this.database.SortedSetRangeByRank(key, start, stop, order);
            List<string> result = null;
            if (items != null && items.Any())
            {
                result = new List<string>();
                foreach (var item in items)
                {
                    result.Add(item.ToString());
                }
            }
            return result;
        }


        public List<string> StringMultiGet(IList<string> keys)
        {
            if (keys == null && keys.Count == 0)
            {
                return null;
            }
            return StringMultiGet(keys.ToArray());
        }


        public List<T> StringMultiGet<T>(IList<string> keys)
        {
            if (keys == null || keys.Count == 0)
            {
                return null;
            }
            List<T> result = null;
            int length = keys.Count;
            var redisKeys = new RedisKey[length];
            for (int i = 0; i < length; i++)
            {
                redisKeys[i] = keys[i];
            }
            RedisValue[] items = this.database.StringGet(redisKeys);
            if (items != null && items.Any())
            {
                result = new List<T>();
                foreach (var item in items)
                {
                    result.Add(serializer.Deserialize<T>(item.ToString()));
                }
            }
            return result;
        }

        public void Remove(params string[] arrayKey)
        {
            if (arrayKey == null || arrayKey.Length == 0)
            {
                return;
            }
            int length = arrayKey.Length;
            var redisKeys = new RedisKey[length];
            for (int i = 0; i < length; i++)
            {
                redisKeys[i] = arrayKey[i];
            }
            this.database.KeyDelete(redisKeys);
        }


        public void SetObject<T>(string key, T value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new Exception("Key is empty or null");
            }
            if (value == null)
            {
                throw new ArgumentNullException("Value");
            }
            string redisValue = this.serializer.Serialize(value);
            this.database.StringSet(key, redisValue);
        }


        public int SortedSetCountAllMember(string key)
        {
            return 1;
        }

        public void SortedSetAdd<T>(string key, T item, double score) where T : class
        {
            string member = this.serializer.Serialize<T>(item);
            this.database.SortedSetAdd(key, member, score);
        }


        public async Task SortedSetAddAsync<T>(string key, T item, double score) where T : class
        {
            string member = this.serializer.Serialize<T>(item);
            await this.database.SortedSetAddAsync(key, member, score);
        }


        public async Task<int> SortedSetCountAllMemberAsync(string key)
        {
            return await Task.FromResult<int>(1);
        }


        public List<T> SortedSetGetAllMembers<T>(bool ascending, string key, int start = 0, int stop = -1) where T : class
        {
            if (key == null)
            {
                return null;
            }
            Order order = (ascending ? Order.Ascending : Order.Descending);
            RedisValue[] items = this.database.SortedSetRangeByRank(key, start, stop, order);
            List<T> result = null;
            if (items != null && items.Any())
            {
                result = new List<T>();
                foreach (var item in items)
                {
                    result.Add(this.serializer.Deserialize<T>(item.ToString()));
                }
            }
            return result;
        }


        public async Task<List<T>> wer<T>(bool ascending, string key, int start = 0, int stop = -1) where T : class
        {
            if (key == null)
            {
                return null;
            }
            Order order = (ascending ? Order.Ascending : Order.Descending);
            RedisValue[] items = await this.database.SortedSetRangeByRankAsync(key, start, stop, order);
            List<T> result = null;
            if (items != null && items.Any())
            {
                result = new List<T>();
                foreach (var item in items)
                {
                    result.Add(this.serializer.Deserialize<T>(item.ToString()));
                }
            }
            return await Task.FromResult<List<T>>(result);
        }

    }

}
