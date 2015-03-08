using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tam.Redis
{
    public interface IRedisClient
    {
        bool KeyExists(string key);
        T GetObject<T>(string key);

        dynamic Get(string key);

        void SetObject<T>(string key, T value, TimeSpan expiredIn);

        void StringSet(string key, string value);


        string StringGet(string key);


        void SetObject<T>(string key, T value, TimeSpan expiredIn, CommandFlags commandFlag);

        void SetObject<T>(string key, T value);

        void Remove(string key, CommandFlags commandFlag);

        Task<bool> ExistsAsync(string key);

        Task<T> GetObjectAsync<T>(string key);

        Task SetObjectAsync<T>(string key, T value, TimeSpan expiredIn);

        Task SetObjectAsync<T>(string key, T value, TimeSpan expiredIn, CommandFlags commandFlags);

        Task RemoveAsync(string key, CommandFlags commandFlags = CommandFlags.None);

        /// <summary>
        /// Get object async
        /// </summary>
        /// <typeparam name="T">Type of object. Ex: string, int, DataTable</typeparam>
        /// <param name="key">key</param>
        /// <param name="expiredIn">expired time of object in minute</param>
        /// <param name="fetch">if object is not existing in Redis Cache, we will use this method to get data</param>
        /// <returns></returns>
        Task<T> GetObjectAsync<T>(string key, TimeSpan expiredIn, Func<T> fetch);

        T GetObject<T>(string key, TimeSpan expiredIn, Func<T> fetch);

        long StringIncrement(string key, long value);

        double StringIncrement(string key, double value);

        void SortedSetAdd(string key, string memberName, double score);

        double? SortedSetGetScore(string key, string memberName);

        void SortedSetIncrement(string key, string memberName, double score);

        void SortedSetIncrementOne(string key, string memberName);

        Task<double?> SortedSetGetScoreAsync(string key, string memberName);

        IDictionary<string, double> SortedSetGetAllMembersWithScore(string key);

        List<string> StringMultiGet(params string[] arrayKey);

        List<string> StringMultiGet(IList<string> keys);

        List<T> StringMultiGet<T>(IList<string> keys);

        IDictionary<string, string> StringMultiGetWithKey(params string[] arrayKey);

        List<string> SortedSetGetAllMembers(bool ascending, string key, int start, int stop);

        void Remove(params string[] arrayKey);

        int SortedSetCountAllMember(string key);
    }
}