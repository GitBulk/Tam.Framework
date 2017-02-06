using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Tam.NetCore.Utilities
{
    public static class HttpContextExtension
    {
        public static IHttpConnectionFeature GetHttpConnectionFeature(this HttpContext context)
        {
            return context.Features.Get<IHttpConnectionFeature>();
        }

        public static void AddHttpResponseHeader(this HttpContext context, string key, string value)
        {
            Guard.ThrowIfNullOrWhiteSpace(key);
            context.Response.Headers.Add(key, value);
        }
        public static void AddHttpResponseHeader(this ResultExecutingContext context, string key, string value)
        {
            context.HttpContext.AddHttpResponseHeader(key, value);
        }
    }
}
