using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Tam.WebApi.Extension
{

    public class BaseRestfulHttpClient<T, TResourceIdentifier> : IDisposable, IBaseRestfulHttpClient<T, TResourceIdentifier> where T : class
    {
        // http://www.matlus.com/a-generic-restful-crud-httpclient/
        // http://stackoverflow.com/questions/10642528/how-does-one-configure-httpclient-not-to-automatically-redirect-when-it-receives

        private bool disposed = false;
        private HttpClient httpClient;
        private readonly string requestApiSuffix;
        protected readonly string serviceBaseAddress;
        private const string Gzip = "gzip";
        private const string Defalte = "defalte";
        protected const string JsonMediaType = "application/json";
        public string UserAgent { get; set; }
        public BaseRestfulHttpClient(string serviceBaseAddress, string requestApiSuffix)
        {
            this.serviceBaseAddress = serviceBaseAddress;
            this.requestApiSuffix = requestApiSuffix;
            this.httpClient = SetupHttpClient(serviceBaseAddress);
        }

        protected virtual ObjectContent CreateJsonObjectContent(T model)
        {
            //var requestMessage = new HttpRequestMessage();
            ObjectContent<T> content = new ObjectContent<T>(model,
                new JsonMediaTypeFormatter(),
                MediaTypeHeaderValue.Parse(JsonMediaType));
            return content;
        }

        protected virtual HttpClient SetupHttpClient(string serviceBaseAddress)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(serviceBaseAddress);
            client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(JsonMediaType));
            client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("defalte"));
            if (string.IsNullOrWhiteSpace(UserAgent))
            {
                UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:33.0) Gecko/20100101 Firefox/33.0";
            }
            client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
            return client;
        }

        public async Task<IEnumerable<T>> GetManyAsync()
        {
            var responseMessage = await httpClient.GetAsync(requestApiSuffix);
            //responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadAsAsync<IEnumerable<T>>();
        }

        public async Task<T> GetAsync(TResourceIdentifier identifier)
        {
            var responseMessage = await httpClient.GetAsync(requestApiSuffix + identifier.ToString());
            //responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadAsAsync<T>();
        }

        public async Task<T> PostAsync(T model)
        {
            var requestMessage = new HttpRequestMessage();
            var objectContent = CreateJsonObjectContent(model);
            var responseMessage = await httpClient.PostAsync(requestApiSuffix, objectContent);
            return await responseMessage.Content.ReadAsAsync<T>();
        }

        public async Task DeleteAsync(TResourceIdentifier identifier)
        {
            await httpClient.DeleteAsync(requestApiSuffix + identifier.ToString());
        }

        public async Task PutAsync(TResourceIdentifier identifier, T model)
        {
            var requestMessage = new HttpRequestMessage();
            var objectContent = CreateJsonObjectContent(model);
            await httpClient.PutAsync(requestApiSuffix + identifier.ToString(), objectContent);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                if (httpClient != null)
                {
                    var hc = httpClient;
                    httpClient = null;
                    hc.Dispose();
                }
                disposed = true;
            }
        }
    }
}