using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Redis
{
    public static class RedisTransactionExtensions
    {
        //Task<long> SortedSetRemoveRangeByRankAsync(RedisKey key, long start, long stop, CommandFlags flags = CommandFlags.None);
        public static async Task<long> SortedSetRemoveAtAsync(this ITransaction trans, RedisKey key, int index)
        {
            return await trans.SortedSetRemoveRangeByRankAsync(key, index, index);
        }
    }
}
