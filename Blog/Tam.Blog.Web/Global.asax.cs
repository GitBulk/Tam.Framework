using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Tam.Blog.Web.App_Start;
using Tam.Control;

namespace Tam.Blog.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //DIConfig.Register();
            OnlyUseRazorViewEngine();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private static void OnlyUseRazorViewEngine()
        {
            System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(new CustomVirtualProviderPath());
            var engine = new RazorViewEngine();
            //engine.ViewLocationFormats = new[] { "~/bin/Views/{1}/{0}.cshtml", "~/Views/Shared/{0}.cshtml" };
            //engine.PartialViewLocationFormats = engine.ViewLocationFormats;
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(engine);
        }
    }
}
