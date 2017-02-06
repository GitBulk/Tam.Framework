using System.Collections.Generic;

namespace Tam.NetCore.Filters.RequestFiltering.IP
{
    public class IPAddressOptions: IRequestFilterOptions
    {
        public List<string> IPAddresses { get; set; }
    }
}
