using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tam.NetCore.Utilities
{
    public static class EntityFrameworkCoreExtension
    {
        public static IServiceCollection AddSqlServerDbContext<T>(this IServiceCollection service, string connectionString) where T : DbContext
        {
            service.AddDbContext<T>(o => o.UseSqlServer(connectionString));
            return service;
        }
    }
}
