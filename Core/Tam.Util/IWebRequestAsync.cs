using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Util
{
    public interface IWebRequestAsync
    {
        string BaseAddress { get; set; }
        Task<string> GetStringAsync(string requestApi);

        Task<HttpResponseMessage> GetResponseAsync(string requestApi);
        Task<string> PostAsync(string requestApi);
    }
}
