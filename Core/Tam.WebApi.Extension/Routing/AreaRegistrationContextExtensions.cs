using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;

namespace Tam.WebApi.Extension.Routing
{
    public static class AreaRegistrationContextExtensions
    {
        public static Route MapHttpRoute(this AreaRegistrationContext context, string name, string routeTemplate, object defaults,
            object constraints = null, string[] namespaces = null)
        {
            // http://blogs.infosupport.com/asp-net-mvc-4-rc-getting-webapi-and-areas-to-play-nicely/
            var route = context.Routes.MapHttpRoute(name, routeTemplate, defaults, constraints);
            if (route.DataTokens == null)
            {
                route.DataTokens = new RouteValueDictionary();
            }
            if (namespaces != null && namespaces.Length > 0)
            {
                route.DataTokens.Add("namespaces", namespaces);
            }
            route.DataTokens.Add("area", context.AreaName);
            return route;
        }

        // usage
        // 1.Create a sample area like below area
        //public class RedAreaRegistration : AreaRegistration
        //{
        //    public override string AreaName
        //    {
        //        get
        //        {
        //            return "Red";
        //        }
        //    }

        //    public override void RegisterArea(AreaRegistrationContext context)
        //    {
        //        // http://blogs.infosupport.com/asp-net-mvc-4-rc-getting-webapi-and-areas-to-play-nicely/
        //        bool enable = false;
        //        if (enable)
        //        {
        //            //string name, string routeTemplate, object defaults
        //            context.MapHttpRoute(
        //                name: "Red_WebApiRoute",
        //                routeTemplate: "Red/Api/{controller}/{id}",
        //                defaults: new { id = RouteParameter.Optional }
        //            );
        //        }


        //        // this is for MVC
        //        context.MapRoute(
        //            "Red_default",
        //            "Red/{controller}/{action}/{id}",
        //            new { action = "Index", id = UrlParameter.Optional }
        //        );

        //    }
        //}
    }
}