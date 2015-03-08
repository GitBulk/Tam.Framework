using NLog;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Redis
{
    // https://ruhul.wordpress.com/2014/07/23/use-redis-as-cache-provider/
    public class SingletonRedisConnection : IRedisConnection
    {
        static Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly string redisConnectionString;
        private static ConnectionMultiplexer instance;
        private static volatile object LockObject = new object();

        public SingletonRedisConnection()
        {
            this.redisConnectionString = "";
        }

        public SingletonRedisConnection(ConfigurationOptions options)
        {
            this.redisConnectionString = options.ToString();
        }

        public SingletonRedisConnection(string redisConnectionString)
        {
            this.redisConnectionString = redisConnectionString;
        }

        public StackExchange.Redis.ConnectionMultiplexer Create()
        {
            Logger.Info("Connect to Redis.");
            if (instance != null && instance.IsConnected)
            {
                Logger.Info("Redis is connected 1");
                return instance;
            }
            lock (LockObject)
            {
                if (instance != null && instance.IsConnected)
                {
                    Logger.Info("Redis is connected 2");
                    return instance;
                }
                if (instance != null)
                {
                    instance.Dispose();
                    Logger.Info("Connection disconnected. Disposing connection...");
                }
                instance = ConnectionMultiplexer.Connect(redisConnectionString);
                Logger.Info("Creating new instance of Redis Connection");
            }
            return instance;
        }



        public void Dispose()
        {
            if (instance != null)
            {
                instance.Dispose();
            }
        }

        public bool IsConnectedToServer()
        {
            return (instance != null && instance.IsConnected);
        }


        public void Close()
        {
            if (this.IsConnectedToServer())
            {
                instance.Close();
            }
        }

        /// <summary>
        /// Close connection to Redis server async
        /// </summary>
        /// <returns></returns>
        public async Task CloseAsync()
        {
            if (this.IsConnectedToServer())
            {
                await instance.CloseAsync();
            }
        }
    }
}
