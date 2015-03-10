using Ninject.Modules;
using System.Data.Entity;
using Tam.Blog.Model;
using Tam.Blog.Repository.Implementation;
using Tam.Blog.Repository.Interface;
using Tam.Repository.Implementation;
using Tam.Repository.Interface;

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
