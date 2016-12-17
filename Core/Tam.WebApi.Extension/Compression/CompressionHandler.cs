using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tam.Util;

namespace Tam.WebApi.Extension.Compression
{
    public class CompressionHandler : DelegatingHandler
    {
        public Collection<ICompressor> Compressors { get; private set; }

        public CompressionHandler()
        {
            this.Compressors = new Collection<ICompressor>();
            this.Compressors.Add(new GZipCompressor());
            this.Compressors.Add(new DeflateCompressor());
        }

        public CompressionHandler(params ICompressor[] compressors)
        {
            Guard.ThrowIfNull(compressors);
            this.Compressors = new Collection<ICompressor>();
            foreach (var item in compressors)
            {
                this.Compressors.Add(item);
            }
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (request.Headers.AcceptEncoding != null && request.Headers.AcceptEncoding.Count > 0 &&
                response.Content != null)
            {
                var query = from encoding in request.Headers.AcceptEncoding
                            let quality = encoding.Quality ?? 1.0
                            where quality > 0
                            join c in Compressors on encoding.Value.ToLower() equals c.EncodingType.ToLower()
                            orderby quality descending
                            select c;
                string defaultCompressor = this.Compressors[0].EncodingType;
                ICompressor compressor = null;
                foreach (var item in query)
                {
                    if (item.EncodingType == defaultCompressor)
                    {
                        compressor = item;
                        break;
                    }
                }
                if (compressor == null)
                {
                    compressor = (query).FirstOrDefault();
                }

                // must check compressor again because it can be still null
                if (compressor != null)
                {
                    response.Content = new CompressedContent(response.Content, compressor);
                }
            }
            return response;
        }
    }
}
