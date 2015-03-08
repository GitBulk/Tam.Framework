using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Blog.DI
{
    public static class DIRegister
    {
        public static void LoadModules(IKernel kernel)
        {
            var modules = new NinjectModule[]
            {
                new DatabaseModule(),
                new CacheModule(),
                new ServiceModule(),
                new RepositoryModule()
            };
            kernel.Load(modules);
        }
    }
}
