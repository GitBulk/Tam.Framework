using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;

namespace Tam.Mvc.Extension.Results
{
    public class FeedResult : ActionResult
    {
        public Encoding ContentEncoding { get; set; }

        public string ContentType { get; set; }

        public SyndicationFeedFormatter FeedFormatter { get; private set; }

        public FeedResult(SyndicationFeedFormatter feedForamtter)
        {
            this.FeedFormatter = feedForamtter;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context is null.");
            }

            var respone = context.HttpContext.Response;
            respone.ContentType = (string.IsNullOrEmpty(this.ContentType) ? "application/rss+xml" : this.ContentType);
            if (this.ContentEncoding != null)
            {
                respone.ContentEncoding = this.ContentEncoding;
            }

            if (this.FeedFormatter != null)
            {
                using (var xmlWriter = new XmlTextWriter(respone.Output))
                {
                    xmlWriter.Formatting = Formatting.Indented;
                    this.FeedFormatter.WriteTo(xmlWriter);
                }
            }
        }
    }
}
