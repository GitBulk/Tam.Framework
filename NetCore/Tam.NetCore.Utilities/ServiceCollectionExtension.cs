using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Tam.NetCore.Utilities
{
    public static class ServiceCollectionExtension
    {

        public static TConfig ConfigPOCO<TConfig>(this IServiceCollection service, IConfiguration configuration) where TConfig : class, new()
        {
            Guard.ThrowIfNull(service);
            Guard.ThrowIfNull(configuration);
            var config = new TConfig();
            configuration.Bind(config);
            service.AddSingleton(config);
            return config;
        }

        public static TConfig ConfigPOCO<TConfig>(this IServiceCollection service, IConfiguration configuration,
            Func<TConfig> pocoProvider) where TConfig : class
        {
            Guard.ThrowIfNull(service);
            Guard.ThrowIfNull(configuration);
            Guard.ThrowIfNull(pocoProvider);
            var config = pocoProvider();
            configuration.Bind(config);
            service.AddSingleton(config);
            return config;
        }

        public static TConfig ConfigPOCO<TConfig>(this IServiceCollection service, IConfiguration configuration,
            TConfig configObject) where TConfig : class
        {
            Guard.ThrowIfNull(service);
            Guard.ThrowIfNull(configuration);
            Guard.ThrowIfNull(configObject);
            configuration.Bind(configObject);
            service.AddSingleton(configObject);
            return configObject;
        }
    }
}

