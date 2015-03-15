using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
namespace Tam.Util
{
    public static class ReturnUrlHelper
    {
        public const string ReturnUrlKey = "return-url";
        public static MvcHtmlString ReturnTo(this HtmlHelper helper, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return MvcHtmlString.Empty;
            }
            return helper.Hidden(ReturnUrlKey, url);
        }

        public static MvcHtmlString ReturnToReferrer(this HtmlHelper helper)
        {
            if (helper != null && (helper.ViewContext != null && helper.ViewContext.HttpContext != null
                && helper.ViewContext.HttpContext.Request != null
                    && helper.ViewContext.HttpContext.Request.UrlReferrer != null))
            {
                return helper.Hidden(ReturnUrlKey, helper.ViewContext.HttpContext.Request.UrlReferrer.ToString());
            }
            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString ReturnToThis(this HtmlHelper helper)
        {
            if (helper != null && (helper.ViewContext != null && helper.ViewContext.HttpContext != null
                && helper.ViewContext.HttpContext.Request != null
                    && helper.ViewContext.HttpContext.Request.Url != null))
            {
                return helper.Hidden(ReturnUrlKey, helper.ViewContext.HttpContext.Request.Url.ToString());
            }
            return MvcHtmlString.Empty;
        }
    }
}
