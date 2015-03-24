using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace Tam.Control
{
    // http://www.danielroot.info/2013/07/reuse-mvc-views-using-virtual-path.html
    public class CustomVirtualFile : VirtualFile
    {
        private readonly string virtualPath;

        public CustomVirtualFile(string virtualPath)
            : base(virtualPath)
        {
            this.virtualPath = VirtualPathUtility.ToAppRelative(virtualPath);
        }

        //private static string LoadResource(string resourceKey)
        //{
        //    // Load from your database respository or whatever here...
        //    // Note that the caching is disabled for this content in the virtual path
        //    // provider, so you must cache this yourself in your repository.

        //    // My implementation using my custom service locator that sits on top of
        //    // Ninject
        //    var contentRepository = FrameworkHelper.Resolve<IContentRepository>();

        //    var resource = contentRepository.GetContent(resourceKey);

        //    if (String.IsNullOrWhiteSpace(resource))
        //    {
        //        resource = String.Empty;
        //    }

        //    return resource;
        //}

        public override Stream Open()
        {
            //return resource.ToStream();

            // virtual path is "~/Tam.Control.dll/Views/Welcome.cshtml"
            var parts = this.virtualPath.Split('/');
            string fileDll = parts[1];
            string folderView = parts[2];
            string viewName = parts[3];
            // Get assembly
            //string assemblyName = parts[1];
            //assemblyName = Path.Combine(HttpRuntime.BinDirectory, assemblyName);
            //var assembly = System.Reflection.Assembly.LoadFile(assemblyName);
            // or
            var assembly = this.GetType().Assembly;
            // assemblyName is "E:\\Project2\\Blog2\\trunk\\Tam.Framework\\Blog\\Tam.Blog.Web\\bin\\Tam.Control.dll"


            if (assembly != null)
            {
                // resourceName must be "Tam.Control.Views.Welcome.cshtml")
                //string resourceName = string.Format("{0}.{1}.{2}", fileDll.Replace(".dll", ""), folderView, viewName);
                string resourceName = this.virtualPath.Replace(".dll", "").Replace("~/", "").Replace("/", ".");

                resourceName = "Tam.Control.Views." + viewName;
                Stream stream = assembly.GetManifestResourceStream(resourceName);
                return stream;
            }
            return null;
        }
    }
}
