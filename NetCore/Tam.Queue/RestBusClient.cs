using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tam.Queue
{
    public class RestBusClient : BaseMessageInvoker
    {
        public RestBusClient(HttpMessageHandler handler, bool disposeHandler) : base(handler, disposeHandler)
        {
        }
    }
}
