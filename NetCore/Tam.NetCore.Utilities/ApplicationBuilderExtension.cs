using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using System.Collections.Generic;
using System.Globalization;

namespace Tam.NetCore.Utilities
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder AddCultures(this IApplicationBuilder app, IList<CultureInfo> cultures, RequestCulture
            defaultCulture)
        {
            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                new CultureInfo("vi-VN")
            };
            if (cultures != null && cultures.Count > 0)
            {
                foreach (var item in cultures)
                {
                    if (supportedCultures.Find(c => c.Name == item.Name) == null)
                    {
                        supportedCultures.Add(item);
                    }
                }
            }
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = defaultCulture,
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            return app;
        }
    }
}
