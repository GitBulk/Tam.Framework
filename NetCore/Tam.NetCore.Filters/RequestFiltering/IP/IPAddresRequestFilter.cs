using Tam.NetCore.Utilities;

namespace Tam.NetCore.Filters.RequestFiltering.IP
{
    public class IPAddresRequestFilter : RequestFilter<IPAddressOptions>
    {
        public override IPAddressOptions Options
        {
            get;
        }

        public IPAddresRequestFilter(IPAddressOptions options)
        {
            this.Options = options;
        }

        public override void ApplyFilter(RequestFilteringContext context)
        {
            var connectionFeature = context.HttpContext.GetHttpConnectionFeature();
            if (connectionFeature == null)
            {
                context.Result = RequestFilterResult.Continue;
            }
            var userIp = connectionFeature.RemoteIpAddress.ToString();
            if (this.Options.IPAddresses.Contains(userIp))
            {
                context.HttpContext.Response.StatusCode = 404;
                context.Result = RequestFilterResult.StopFilters;
            }
            else
            {
                context.Result = RequestFilterResult.Continue;
            }
        }
    }
}
