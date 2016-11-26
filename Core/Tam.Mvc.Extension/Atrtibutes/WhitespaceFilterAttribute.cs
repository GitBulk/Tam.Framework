using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Tam.Mvc.Extension.Atrtibutes
{
    /// <summary>
    /// Remove all white space of web page (Right mouse on web page and select View Source to see result)
    /// Usage: GlobalFilters.Filters.Add(new WhitespaceFilterAttribute()); or add/remove filter through configuration
    /// </summary>
    public class WhitespaceFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            if (filterContext.HttpContext.Request.RawUrl == "/sitemap.xml")
            {
                return;
            }
            if (response.ContentType != "text/html" || response.Filter == null)
            {
                return;
            }
            response.Filter = new ResponseFilterStream(response.Filter);
        }

        private class ResponseFilterStream : Stream
        {
            private readonly Stream responseStream;
            StringBuilder builder = new StringBuilder();

            public override bool CanRead
            {
                get
                {
                    return false;
                }
            }

            public override bool CanSeek
            {
                get
                {
                    return false;
                }
            }

            public override bool CanWrite
            {
                get
                {
                    return true;
                }
            }

            public override long Length
            {
                get
                {
                    throw new NotSupportedException();
                }
            }

            public override long Position
            {
                get
                {
                    throw new NotSupportedException();
                }

                set
                {
                    throw new NotSupportedException();
                }
            }

            public ResponseFilterStream(Stream responseStream)
            {
                if (responseStream == null)
                {
                    throw new ArgumentNullException("responseStream");
                }
                this.responseStream = responseStream;
            }

            public override void Flush()
            {
                this.responseStream.Flush();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotSupportedException();
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                string html = Encoding.UTF8.GetString(buffer, offset, count);
                var reg = new Regex(@"(?<=\s)\s+(?![^<>]*</pre>)");
                html = reg.Replace(html, string.Empty);
                buffer = Encoding.UTF8.GetBytes(html);
                this.responseStream.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
