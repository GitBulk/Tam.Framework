using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Redis
{
    public interface IRedisServerSettings
    {
        bool PreferSlaveForRead { get; }
        string ConnectionStringOrName { get; }
        int DefaultDb { get; }
    }
}
