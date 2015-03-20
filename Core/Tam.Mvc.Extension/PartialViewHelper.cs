using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Tam.Mvc.Extension
{
    public static class PartialViewHelper
    {
        public static string RenderToString(this PartialViewResult partialView)
        {
            // http://stackoverflow.com/questions/2537741/how-to-render-partial-view-into-a-string
            var httpContext = HttpContext.Current;
            if (httpContext == null)
            {
                throw new NotSupportedException("An HTTP context is required to render the partial view to a string");
            }
            var controllerName = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
            var controller = ControllerBuilder.Current.GetControllerFactory()
                .CreateController(httpContext.Request.RequestContext, controllerName) as ControllerBase;
            var controllerContext = new ControllerContext(httpContext.Request.RequestContext, controller);
            var view = ViewEngines.Engines.FindPartialView(controllerContext, partialView.ViewName).View;
            var builder = new StringBuilder();
            using (var sw = new StringWriter(builder))
            {
                using (var tw = new HtmlTextWriter(sw))
                {
                    view.Render(new ViewContext(controllerContext, view, partialView.ViewData,
                        partialView.TempData, sw), sw);
                }
            }
            return builder.ToString();

            // usage:
            // var html = PartialView("ViewName").RenderToString();
        }
    }
}
