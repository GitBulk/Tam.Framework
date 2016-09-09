using System;
using System.IO;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace Tam.Mvc.Atrtibutes
{
    public class ETagAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Filter = new ETagFilter(filterContext.HttpContext.Response, filterContext.RequestContext.HttpContext.Request);
        }

        private class ETagFilter : MemoryStream
        {
            private HttpRequestBase request;
            private HttpResponseBase response;
            private Stream filterStream;
            public ETagFilter(HttpResponseBase response, HttpRequestBase request)
            {
                this.response = response;
                this.request = request;
                this.filterStream = response.Filter;
            }

            private string GetToken(Stream stream)
            {
                var checksum = new byte[0];
                checksum = MD5.Create().ComputeHash(this.filterStream);
                return Convert.ToBase64String(checksum, 0, checksum.Length);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                var data = new byte[count];
                Buffer.BlockCopy(buffer, offset, data, 0, count);
                var token = GetToken(new MemoryStream(data));
                var clientToken = this.request.Headers["If-None-Match"];
                if (token != clientToken)
                {
                    this.response.AddHeader("ETag", token);
                    this.filterStream.Write(data, 0, count);
                }
                else
                {
                    this.response.SuppressContent = true;
                    this.response.StatusCode = 304; // http status code 304: Not modified
                    this.response.StatusDescription = "Not Modified";
                    this.response.AddHeader("Content-Length", "0");
                }
                base.Write(buffer, offset, count);
            }
        }
    }
}
