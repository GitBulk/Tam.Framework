using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tam.Blog.ServiceLayer.Implementation;
using Tam.Blog.ServiceLayer.Interface;

using Ninject.Web.Mvc;
using Ninject.Web.Common;
using Tam.Blog.DI;

namespace Tam.Blog.Web
{
    public class DIConfig
    {
        // we can use NinjectWebCommon.cs

        public static void Register()
        {
            string conStr = ConfigurationManager.ConnectionStrings["GreatBlogSql"].ToString();
            IKernel kernel = new StandardKernel();
            //IKernel kernel = bootstrapper.Kernel;
            //kernel.Bind<ISqlServerHelper>().To<SqlServerHelper>().WithConstructorArgument("connectionString", conStr);
            DIRegister.LoadModules(kernel);

            // must install Nuget ninject.mvc5
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}