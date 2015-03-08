using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Modules;
using Tam.Blog.ServiceLayer.Interface;
using Tam.Blog.ServiceLayer.Implementation;

namespace Tam.Blog.DI
{
    class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPostService>().To<PostService>();
            Bind<IUserService>().To<UserService>();
        }
    }
}
