using Microsoft.AspNetCore.Builder;
using Microsoft.Net.Http.Headers;
using System;

namespace Tam.NetCore.Util
{
    /// <summary>
    /// Usage: app.UseStaticFiles(new TimeStaticFileOptions(a number)
    /// </summary>
    public class TimeStaticFileOptions : StaticFileOptions
    {
        public TimeStaticFileOptions(int durationInSeconds = 60) : base()
        {
            if (durationInSeconds < 1)
            {
                throw new Exception("Dutaion cannot less than one second.");
            }
            this.OnPrepareResponse = ctx =>
            {
                ctx.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=" + durationInSeconds;
            };
        }
    }
}
