using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tam.Blog.Cache.Implementation;
using Tam.Blog.Cache.Interface;

namespace Tam.Blog.DI
{
    class CacheModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPostCache>().To<PostCache>();
        }
    }
}
