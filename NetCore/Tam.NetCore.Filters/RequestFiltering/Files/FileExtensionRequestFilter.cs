using System.IO;
using System.Linq;

namespace Tam.NetCore.Filters.RequestFiltering.Files
{
    public class FileExtensionRequestFilter : RequestFilter<FileExtensionsOptions>
    {
        public override FileExtensionsOptions Options
        {
            get;
        }

        public FileExtensionRequestFilter(FileExtensionsOptions options)
        {
            this.Options = options;
        }

        public override void ApplyFilter(RequestFilteringContext context)
        {
            var extension = Path.GetExtension(context.HttpContext.Request.Path.Value);
            if (Options.AllowUnlisted)
            {
                if (this.Options.FileExtensionCollection.Any(f => f.FileExtension == extension && f.Allowed == false))
                {
                    context.HttpContext.Response.StatusCode = 404;
                    context.Result = RequestFilterResult.StopFilters;
                }
                else
                {
                    context.Result = RequestFilterResult.Continue;
                }
            }
            else
            {
                if (this.Options.FileExtensionCollection.Any(f => f.FileExtension == extension && f.Allowed == true))
                {
                    context.Result = RequestFilterResult.Continue;
                }
                else
                {
                    context.HttpContext.Response.StatusCode = 404;
                    context.Result = RequestFilterResult.StopFilters;
                }
            }
        }
    }
}
