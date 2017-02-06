using Microsoft.AspNetCore.Http;
using System.Net;

namespace Tam.NetCore.Utilities
{
    public static class HttpRequestExtension
    {
        public static bool IsLocal(this HttpRequest request)
        {
            var connection = request.HttpContext.Connection;
            if (connection.RemoteIpAddress != null)
            {
                if (connection.RemoteIpAddress.ToString() == "::1")
                {
                    return true;
                }
                if (connection.LocalIpAddress != null)
                {
                    if (connection.LocalIpAddress.ToString() == "127.0.0.1")
                    {
                        return true;
                    }
                    return connection.RemoteIpAddress.Equals(connection.LocalIpAddress);
                }
                return IPAddress.IsLoopback(connection.RemoteIpAddress);
            }
            if (connection.RemoteIpAddress == null && connection.LocalIpAddress == null)
            {
                return true;
            }
            return false;
        }
    }
}
