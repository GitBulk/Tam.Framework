using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace Tam.Control
{

    // http://www.danielroot.info/2013/07/reuse-mvc-views-using-virtual-path.html
    public class CustomVirtualProviderPath : VirtualPathProvider
    {
        private readonly Assembly assembly = typeof(CustomVirtualProviderPath).Assembly;

        private readonly string[] resourceNames;

        public CustomVirtualProviderPath()
        {
            this.resourceNames = assembly.GetManifestResourceNames();
        }

        private bool IsEmbeddedResourcePath(string virtualPath)
        {
            var checkPath = VirtualPathUtility.ToAppRelative(virtualPath);
            return checkPath.StartsWith("~/Tam.Control", StringComparison.InvariantCultureIgnoreCase);

            // or

            //var checkPath = VirtualPathUtility.ToAppRelative(virtualPath);
            //var resourceName = this.GetType().Namespace + "." + checkPath.Replace("~/", "").Replace("/", ".");
            //return this.resourceNames.Contains(resourceName);
        }

        public override bool FileExists(string virtualPath)
        {
            bool fileExists = base.FileExists(virtualPath);
            bool isEmbededFile = IsEmbeddedResourcePath(virtualPath);
            bool result = (isEmbededFile || fileExists);
            return result;
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (IsEmbeddedResourcePath(virtualPath))
            {
                return new CustomVirtualFile(virtualPath);
            }
            else
            {
                return base.GetFile(virtualPath);
            }
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (IsEmbeddedResourcePath(virtualPath))
            {
                return null;
            }
            return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }
    }
}
