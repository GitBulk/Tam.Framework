using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tam.Control.BootstrapControls;
using System.Web.WebPages;
using System.Web.Mvc.Html;
using System.Web;
using NLog;
using System.Web.Routing;
using Tam.Control.BootstrapControls.Pagination;


namespace Tam.Control
{
    public static class HtmlControlExtensions
    {
        private static Logger Log = LogManager.GetLogger(typeof(HtmlControlExtensions).FullName);


        const string ViewPathFormat = "~/Tam.Control.dll/Views/{0}.cshtml";
        public static BootStrapHelper BootStrap(this HtmlHelper helper)
        {
            return new BootStrapHelper();
        }

        public static MvcHtmlString AlphaPager(this HtmlHelper helper, PagerSize pagingSize, string selectedText,
            Func<string, string> pageLink)
        {
            List<string> alphabet = Enumerable.Range(65, 26).Select(i => ((char)i).ToString()).ToList();
            alphabet.Insert(0, "All");
            alphabet.Insert(1, "0-9");
            var builder = new StringBuilder();

            string classPagingAttribute = ""; // medium size
            if (pagingSize == PagerSize.Large)
            {
                classPagingAttribute = "pagination-lg";
            }
            else if (pagingSize == PagerSize.Small)
            {
                classPagingAttribute = "pagination-sm";
            }
            builder.Append(string.Format("<ul class=\"pagination alpha {0}\">", classPagingAttribute));
            foreach (var letter in alphabet)
            {
                if (string.Equals(letter, selectedText, StringComparison.OrdinalIgnoreCase))
                {
                    builder.Append(string.Format("<li class=\"active\"><span>{0}</span></li>", letter));
                }
                else
                {
                    builder.Append("<li>");
                    var a = new TagBuilder("a");
                    string href = pageLink(letter);
                    a.MergeAttribute("href", href);
                    a.InnerHtml = letter;
                    builder.Append(a.ToString());
                    builder.Append("</li>");
                }
            }
            builder.Append("</ul>");
            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString Welcome(this HtmlHelper helper)
        {
            try
            {
                string viewName = GetFullPath("Welcome", helper);
                return helper.Partial(viewName);
            }
            catch (Exception ex)
            {
                var builder = new StringBuilder();
                while (true)
                {
                    builder.AppendLine(ex.Message);
                    Exception exc = ex.InnerException;
                    if (exc == null)
                    {
                        break;
                    }
                }
                Log.Error(builder.ToString());
                //Console.WriteLine(builder.ToString());
                return helper.Partial("Welcome.cshtml");
            }
        }

        private static string GetFullPath(string nameOfView, HtmlHelper helper)
        {
            string viewPath = string.Format(ViewPathFormat, nameOfView);
#if DEBUG
            bool exists = ViewExists(viewPath, helper);
#endif
            return viewPath;
        }

        private static bool ViewExists(string viewPath, HtmlHelper helper)
        {
            var context = helper.ViewContext.Controller.ControllerContext;
            ViewEngineResult result = ViewEngines.Engines.FindView(context, viewPath, null);
            string to = result.ToString();
            return (result.View != null);
        }
    }
}
