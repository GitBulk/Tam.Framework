using System;
using System.Collections.Generic;

namespace Tam.Queue
{
    public class RabbitMQMessagingProperties: RequestMessagingProperties
    {

        public bool? Persitent { get; set; }

        public string RoutingKey { get; set; }
        public TimeSpan Expiration { get; set; }
        public ExchangeKind? ExchangeKind { get; set; }
        public IDictionary<string, object> Headers { get; set; }
    }
}
