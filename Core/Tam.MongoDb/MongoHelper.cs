using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tam.MongoDb.Interface;

namespace Tam.MongoDb
{
    public static class MongoHelper
    {
        private static MongoServer GetMongoServer(IMongoSetting setting)
        {
            var client = new MongoClient(setting.ConnectionString);
            MongoServer server = client.GetServer();
            return server;
        }

        public static MongoDatabase GetDatabase(string databaseName, IMongoSetting setting)
        {
            MongoDatabase db = GetMongoServer(setting).GetDatabase(databaseName);
            return db;
        }

        public static IEnumerable<string> GetDatabaseNames(IMongoSetting setting)
        {
            MongoServer server = GetMongoServer(setting);
            return server.GetDatabaseNames();
        }

        public static void DeleteDatabase(string databaseName, IMongoSetting setting)
        {
            MongoDatabase db = GetDatabase(databaseName, setting);
            db.Drop();
        }

        public static IEnumerable<string> GetCollectionNames(string databaseName, IMongoSetting setting)
        {
            MongoDatabase db = GetDatabase(databaseName, setting);
            return db.GetCollectionNames();
        }

        public static bool CreateCollection(string databaseName, IMongoSetting setting, string collectionName)
        {
            MongoDatabase db = GetDatabase(databaseName, setting);
            CommandResult result = db.CreateCollection(collectionName);
            return (result.Ok ? true : false);
        }

        public static bool DropCollection(string databaseName, IMongoSetting setting, string collectionName)
        {
            MongoDatabase db = GetDatabase(databaseName, setting);
            CommandResult result = db.DropCollection(collectionName);
            return (result.Ok ? true : false);
        }

        public static bool CollectionExists(string databaseName, IMongoSetting setting, string collectionName)
        {
            MongoDatabase db = GetDatabase(databaseName, setting);
            return db.CollectionExists(collectionName);
        }
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
