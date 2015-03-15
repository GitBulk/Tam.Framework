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
            return TestConnection(server);
        }

        private static bool TestConnection(MongoServer server)
        {
            bool result = false;
            try
            {
                server.Connect();
                if (server.State == MongoServerState.Connected || server.State == MongoServerState.ConnectedToSubset)
                {
                    result = true;
                }
            }
            finally
            {
                server.Disconnect();
            }
            return result;
        }
    }
}
