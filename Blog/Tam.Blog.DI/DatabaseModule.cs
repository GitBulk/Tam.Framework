using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tam.Database;

namespace Tam.Blog.DI
{
    class DatabaseModule : NinjectModule
    {
        public override void Load()
        {
            string conStr = ConfigurationManager.ConnectionStrings["GreatBlogSql"].ToString();

            // we can use fluent WithConstructorArgument to add multi parametter.
            Bind<ISqlServerHelper>().To<SqlServerHelper>().WithConstructorArgument("connectionString", conStr);
        }
    }
}
