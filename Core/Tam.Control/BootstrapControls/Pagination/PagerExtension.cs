using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace Tam.Control.BootstrapControls.Pagination
{
    public static class PagerExtension
    {
        public static MvcHtmlString SetPager(this AjaxHelper ajaxHelper, PagerOptions option, AjaxOptions ajaxOptions, object values)
        {
            if (option.CurentPage < 1)
            {
                return MvcHtmlString.Empty;
            }
            RouteValueDictionary routeDictionary = null;
            if (values != null)
            {
                routeDictionary = new RouteValueDictionary(values);
            }
            else
            {
                routeDictionary = new RouteValueDictionary();
            }
            var pager = new Pager(option, routeDictionary, ajaxHelper.ViewContext, ajaxHelper, ajaxOptions);
            return MvcHtmlString.Create(pager.BuildLinks());
        }

        public static MvcHtmlString SetPager(this HtmlHelper helper, PagerOptions option, object values)
        {
            if (option.CurentPage < 1)
            {
                return MvcHtmlString.Empty;
            }
            RouteValueDictionary routeDictionary = null;
            if (values != null)
            {
                routeDictionary = new RouteValueDictionary(values);
            }
            else
            {
                routeDictionary = new RouteValueDictionary();
            }
            //var pager = new Pager(option, routeDictionary, helper.ViewContext, helper);
            var pager = new Pager(option, routeDictionary, helper.ViewContext);
            return MvcHtmlString.Create(pager.BuildLinks());
        }
    }
}