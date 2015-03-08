using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Redis
{
    public interface IRedisConnectionManager : IDisposable
    {
        IDatabase Database(int? db = null);

        IServer Server(EndPoint endPoint);

        EndPoint[] GetEndpoints();

        void FlushDb(int? db = null);

        void Close();
    }
}
