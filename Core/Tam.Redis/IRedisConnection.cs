using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Redis
{
    public interface IRedisConnection
    {
        ConnectionMultiplexer Create();

        void Dispose();

        bool IsConnectedToServer();

        /// <summary>
        /// Close connection
        /// </summary>
        void Close();

        Task CloseAsync();
    }
}
