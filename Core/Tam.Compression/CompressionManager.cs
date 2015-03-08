using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Tam.Compression
{
    // http://weblog.west-wind.com/posts/2012/Apr/28/GZipDeflate-Compression-in-ASPNET-MVC
    // http://blog.developers.ba/asp-net-web-api-gzip-compression-actionfilter/
    public class CompressionManager
    {
        public static void GZipEncodePage()
        {
            HttpResponse response = HttpContext.Current.Response;
            if (IsGZipSupported())
            {
                string acceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];
                response.Filter = new System.IO.Compression.GZipStream(response.Filter,
                        System.IO.Compression.CompressionMode.Compress);
                response.Headers.Remove("Content-Encoding");
                if (acceptEncoding.Contains("gzip"))
                {
                    response.AppendHeader("Content-Encoding", "gzip");
                }
                else
                {
                    response.AppendHeader("Content-Encoding", "deflate");
                }
            }

            // Allow proxy servers to cache encoded and unencoded versions separately
            response.AppendHeader("Vary", "Content-Encoding");
        }

        public static bool IsGZipSupported()
        {
            string acceptEncoding = HttpContext.Current.Request.Headers["Accept-Enconding"];
            if (string.IsNullOrEmpty(acceptEncoding) == false &&
                (acceptEncoding.Contains("gzip") || acceptEncoding.Contains("deflate")))
            {
                return true;
            }
            return false;
        }
    }
}
