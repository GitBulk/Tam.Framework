using System.Collections.Generic;

namespace Tam.NetCore.Filters.RequestFiltering
{
    public class RequestFiltMaintenanceWindowerOptions
    {
        public IList<IRequestFilter> Filters { get; set; } = new List<IRequestFilter>();
    }
}
