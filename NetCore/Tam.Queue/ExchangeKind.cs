using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tam.Queue
{
    [Flags]
    public enum ExchangeKind
    {
        Direct = 1,
        Fanout = 2,
        Headers = 4,
        Topic = 8
    }
}
