using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tam.Blog.Web.Startup))]
namespace Tam.Blog.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
