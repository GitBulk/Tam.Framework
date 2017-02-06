using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tam.NetCore.Utilities
{
    public static class IdentityExtension
    {
        public static IServiceCollection AddIdentity<TIdentityUser, TIdentityRole, TContext>(this IServiceCollection services, bool useDefaultTokenProvider = true)
            where TIdentityUser : IdentityUser
            where TIdentityRole : IdentityRole
            where TContext : DbContext
        {

            if (useDefaultTokenProvider)
            {
                services.AddIdentity<TIdentityUser, TIdentityRole>(o => o.Options()).AddEntityFrameworkStores<TContext>().AddDefaultTokenProviders();
            }
            else
            {
                services.AddIdentity<TIdentityUser, TIdentityRole>(o => o.Options()).AddEntityFrameworkStores<TContext>();
            }
            return services;
        }

        public static IdentityOptions Options(this IdentityOptions options)
        {
            options.Lockout.MaxFailedAccessAttempts = 5;
            return options;
        }


        public static IdentityOptions Options(this IdentityOptions options, int maxFailedAccess)
        {
            options.Lockout.MaxFailedAccessAttempts = maxFailedAccess;
            return options;
        }
    }
}
