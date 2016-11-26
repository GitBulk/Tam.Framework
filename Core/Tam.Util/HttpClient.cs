using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using System.Text;
using System.Threading.Tasks;

namespace Tam.Util
{
    public class HttpClient : IWebRequestAsync
    {
        public String BaseAddress { get; set; }
        public HttpClient(string baseAddress)
        {
            this.BaseAddress = baseAddress;
        }

        public Task<string> GetStringAsync(string requestApi)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                SetupHttpClient(requestApi, client);
                return client.GetStringAsync(requestApi);
            }
        }

        public Task<string> PostAsync(string requestApi)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get HttpResponseMEssage async
        /// </summary>
        /// <param name="requestApi">Request api. For example: api/products/1</param>
        /// <returns></returns>
        public Task<HttpResponseMessage> GetResponseAsync(string requestApi)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                SetupHttpClient(requestApi, client);
                client.p
                return client.GetAsync(requestApi);
            }
        }

        private static void SetupHttpClient(string requestApi, System.Net.Http.HttpClient client)
        {
            client.BaseAddress = new Uri(requestApi);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/jsson"));
        }
    }
}
