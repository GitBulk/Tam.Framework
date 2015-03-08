[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Tam.Blog.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Tam.Blog.Web.App_Start.NinjectWebCommon), "Stop")]

namespace Tam.Blog.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using System.Configuration;
    using Tam.Blog.Web.Models;
    using Tam.Blog.DI;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                // install nuget Ninject.Web.WebApi.WebHost, ninject mvc 5
                //Suport WebAPI Injection
                //GlobalConfiguration.Configuration.DependencyResolver = new WebApiContrib.IoC.Ninject.NinjectResolver(kernel);

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //string conStr = ConfigurationManager.ConnectionStrings["GreatBlogSql"].ToString();
            //kernel.Bind<ISqlServerHelper>().To<SqlServerHelper>().WithConstructorArgument("connectionString", conStr);
            //Tam.Blog.DI.DIRegister.LoadModules(kernel);

            DIRegister.LoadModules(kernel);
        }
    }
}
