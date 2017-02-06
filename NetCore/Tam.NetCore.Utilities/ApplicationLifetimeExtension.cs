using Microsoft.AspNetCore.Hosting;
using System;

namespace Tam.NetCore.Utilities
{
    public static class ApplicationLifetimeExtension
    {
        public static void Started(this IApplicationLifetime lifeTime, Action callback)
        {
            lifeTime.ApplicationStarted.Register(callback);
        }

        public static void Stopping(this IApplicationLifetime lifeTime, Action callback)
        {
            lifeTime.ApplicationStopping.Register(callback);
        }

        public static void Stopped(this IApplicationLifetime lifeTime, Action callback)
        {
            lifeTime.ApplicationStopped.Register(callback);
        }
    }
}
