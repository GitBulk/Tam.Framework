using Tam.NetCore.Filters.RequestFiltering.Files;
using Tam.NetCore.Filters.RequestFiltering.IP;
using Tam.NetCore.Utilities;

namespace Tam.NetCore.Filters.RequestFiltering
{
    public static class RequestFilterOptionsExtensions
    {
        public static RequestFiltMaintenanceWindowerOptions AddRequestFilter(this RequestFiltMaintenanceWindowerOptions options, IRequestFilter filter)
        {
            Guard.ThrowIfNull(filter);
            options.Filters.Add(filter);
            return options;
        }


        public static RequestFiltMaintenanceWindowerOptions AddIpFilter(this RequestFiltMaintenanceWindowerOptions filterOptions, IPAddressOptions options)
        {
            Guard.ThrowIfNull(options);
            var filter = new IPAddresRequestFilter(options);
            filterOptions.AddRequestFilter(filter);
            return filterOptions;
        }

        public static RequestFiltMaintenanceWindowerOptions AddFileFilter(this RequestFiltMaintenanceWindowerOptions filterOptions, FileExtensionsOptions options)
        {
            Guard.ThrowIfNull(options);
            var filter = new FileExtensionRequestFilter(options);
            return filterOptions.AddRequestFilter(filter);
        }
    }
}
