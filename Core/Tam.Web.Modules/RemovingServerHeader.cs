using System;
using System.Web;

namespace Tam.Web.Modules
{
    public class RemovingServerHeader: IHttpModule
    {
        // http://r2d2.cc/2011/10/21/how-to-remove-server-x-aspnet-version-x-aspnetmvc-version-and-x-powered-by-from-the-response-header-in-iis7/
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
        }

        private void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Remove("Server");
        }
    }
}
