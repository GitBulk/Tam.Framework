using Enyim.Caching;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Cache.Memcached
{
    public class MemcachedClientFactory
    {
        static Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static volatile object LockObject = new object();
        private static MemcachedClient Client;

        public MemcachedClientFactory()
        {}

        public MemcachedClient Instance()
        {
            Logger.Info("Start Memcached instance");
            if (Client == null)
            {
                Logger.Info("Memcached is null (1)");
                lock (LockObject)
                {
                    Logger.Info("Lock object memcached");
                    if (Client == null)
                    {
                        Logger.Info("Memcached is null (2)");
                        Client = new MemcachedClient();
                    }
                }
            }
            return Client;
        }
    }
}
