using Enyim.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tam.Cache.Memcached
{
    public interface IMemcachedClientFactory
    {
        MemcachedClient Instance();
    }
}
