using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Tam.Queue
{
    public class BaseMessageInvoker : HttpMessageInvoker
    {
        private const string Request_Key = "Tam_Queue";

        public BaseMessageInvoker(HttpMessageHandler handler, bool disposeHandler)
            : base(handler, disposeHandler)
        {
        }

        private static Uri GetUri(string requestUri)
        {
            if (string.IsNullOrEmpty(requestUri))
            {
                throw new Exception("RequestUri is empty or null.");
            }
            return new Uri(requestUri, UriKind.RelativeOrAbsolute);
        }

        public Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            return GetAsync(requestUri, HttpCompletionOption.ResponseContentRead);
        }

        private Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption option)
        {
            return GetAsync(requestUri, option, CancellationToken.None);
        }

        private Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption option, CancellationToken cancellationToken)
        {
            return SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUri), option, cancellationToken);
        }

        private Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, HttpCompletionOption option, CancellationToken cancellationToken)
        {
            return SendAsync(httpRequestMessage, cancellationToken);
        }

        public Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            return SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUri), cancellationToken);
        }

        public Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            return DeleteAsync(GetUri(requestUri));
        }

        private Task<HttpResponseMessage> DeleteAsync(Uri uri)
        {
            return DeleteAsync(uri, CancellationToken.None);
        }

        /// <summary>Send a DELETE request to the specified Uri with a cancellation token as an asynchronous operation.</summary>
        /// <returns>Returns <see cref="T:System.Threading.Tasks.Task`1" />.The task object representing the asynchronous operation.</returns>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancel">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="requestUri" /> was null.</exception>
        public Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancel)
        {
            return this.SendAsync(new HttpRequestMessage(method: HttpMethod.Delete,
                requestUri: requestUri), cancel);
        }
    }
}
