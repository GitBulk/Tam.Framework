using System.Web;

namespace Tam.Compression
{
    // http://weblog.west-wind.com/posts/2012/Apr/28/GZipDeflate-Compression-in-ASPNET-MVC
    // http://blog.developers.ba/asp-net-web-api-gzip-compression-actionfilter/
    public class CompressionManager
    {

        private const string ContentEncoding = "Content-Encoding";
        private const string AcceptEncoding = "Accept-Encoding";
        private const string GzipMode = "gzip";
        private const string DeflateMode = "deflate";
        private const string VaryHeader = "Vary";

        public static void GZipEncodePage()
        {
            HttpResponse response = HttpContext.Current.Response;
            if (IsGzipSupported())
            {
                string acceptEncoding = HttpContext.Current.Request.Headers[AcceptEncoding];
                if (acceptEncoding.Contains(GzipMode))
                {
                    response.Filter = new System.IO.Compression.GZipStream(response.Filter, System.IO.Compression.CompressionMode.Compress);
                    response.Headers.Remove(ContentEncoding);
                    response.AppendHeader(ContentEncoding, GzipMode);
                }
                else
                {
                    response.Filter = new System.IO.Compression.DeflateStream(response.Filter, System.IO.Compression.CompressionMode.Compress);
                    response.Headers.Remove(ContentEncoding);
                    response.AppendHeader(ContentEncoding, DeflateMode);
                }
            }
            // Allow proxy servers to cache encoded and unencoded versions separately.
            response.AppendHeader(VaryHeader, ContentEncoding);
        }

        public static bool IsGzipSupported()
        {
            string acceptEncoding = HttpContext.Current.Request[AcceptEncoding];
            if (!string.IsNullOrEmpty(acceptEncoding) &&
                (acceptEncoding.Contains(GzipMode) || acceptEncoding.Contains(DeflateMode)))
            {
                return true;
            }
            return false;
        }
    }
}
