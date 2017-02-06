using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace Tam.NetCore.Utilities
{
    public static class ViewExtension
    {
        public static void AddExternalViewComponent<TViewComponent>(this IServiceCollection services, string baseNameSpace = "") where TViewComponent : ViewComponent
        {
            var assembly = typeof(TViewComponent).GetTypeInfo().Assembly;
            //string nameSpace = typeof(TViewComponent).Namespace;
            if (string.IsNullOrWhiteSpace(baseNameSpace))
            {
                baseNameSpace = typeof(TViewComponent).Namespace;
            }
            var embeddedFileProvider = new EmbeddedFileProvider(assembly, baseNameSpace);
            services.Configure<RazorViewEngineOptions>(o =>
            {
                o.FileProviders.Add(embeddedFileProvider);
            });
        }
    }
}
