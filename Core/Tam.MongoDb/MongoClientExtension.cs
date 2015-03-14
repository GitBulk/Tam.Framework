using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.MongoDb
{
    public static class MongoClientExtension
    {
        public static bool TestConnection(this MongoClient client, string connectionString)
        {
            client = new MongoClient(connectionString);
            MongoServer server = client.GetServer();
            return TestConnection(server);
        }

        public static bool TestConnection(this MongoClient client, MongoUrl url)
        {
            client = new MongoClient(url);
            MongoServer server = client.GetServer();
            server.Connect();
            return TestConnection(server);
        }

        private static bool TestConnection(MongoServer server)
        {
            server.Connect();
            if (server.State == MongoServerState.Connected || server.State == MongoServerState.ConnectedToSubset)
            {
                return true;
            }
            return false;
        }
    }
}
