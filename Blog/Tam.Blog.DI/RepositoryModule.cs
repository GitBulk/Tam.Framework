using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tam.Blog.Model;
using Tam.Blog.Repository.Implementation;
using Tam.Blog.Repository.Interface;

namespace Tam.Blog.DI
{
    class RepositoryModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPostRepository>().To<PostRepository>();
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<DbContext>().To<GreatBlogEntities>();
        }
    }
}
