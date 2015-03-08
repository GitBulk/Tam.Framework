using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Redis
{
    public class RedisConnectionManager : IRedisConnectionManager
    {
        private readonly IRedisConnection connection;
        private readonly IRedisServerSettings settings;
        //private readonly Lazy<string> connectionString;
        //private readonly string connectionString;
        //private volatile ConnectionMultiplexer connection;
        private readonly object LockObject = new object();

        public RedisConnectionManager(IRedisServerSettings settings, IRedisConnection redisConnection)
        {
            this.settings = settings;
            //this.connectionString = new Lazy<string>(GetConnectionString);
            //this.connectionString = GetConnectionString();
            this.connection = redisConnection;
        }

        public string ConnectionString
        {
            get
            {
                var con = ConfigurationManager.ConnectionStrings[this.settings.ConnectionStringOrName];
                return con == null ? this.settings.ConnectionStringOrName : con.ConnectionString;
            }
        }

        //private string GetConnectionString()
        //{
        //    var con = ConfigurationManager.ConnectionStrings[this.setting.ConnectionStringOrName];
        //    return con == null ? this.setting.ConnectionStringOrName : con.ConnectionString;
        //}

        //private ConnectionMultiplexer GetConnection()
        //{
        //    return this.connectionFactory.Create();
        //}

        public ConnectionMultiplexer CurrentConnection
        {
            get
            {
                return this.connection.Create();
            }
        }

        public StackExchange.Redis.IDatabase Database(int? db = null)
        {
            //return GetConnection().GetDatabase(db ?? this.setting.DefaultDb);
            return this.CurrentConnection.GetDatabase(db ?? this.settings.DefaultDb);
        }

        public StackExchange.Redis.IServer Server(System.Net.EndPoint endPoint)
        {
            return this.CurrentConnection.GetServer(endPoint);
        }

        public System.Net.EndPoint[] GetEndpoints()
        {
            return this.CurrentConnection.GetEndPoints();
        }

        public void FlushDb(int? db = null)
        {
            var endPoints = GetEndpoints();
            foreach (var endPoint in endPoints)
            {
                this.Server(endPoint).FlushDatabase(db ?? this.settings.DefaultDb);
            }
        }

        public void Dispose()
        {
            this.connection.Dispose();
        }


        public void Close()
        {
            this.connection.Close();
        }
    }
}
