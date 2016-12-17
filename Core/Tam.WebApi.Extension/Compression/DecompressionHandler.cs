using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Tam.Util;

namespace Tam.WebApi.Extension.Compression
{
    public class DecompressionHandler : DelegatingHandler
    {
        public Collection<ICompressor> Compressors { get; private set; }

        public DecompressionHandler()
        {
            this.Compressors = new Collection<ICompressor>();
            this.Compressors.Add(new GZipCompressor());
            this.Compressors.Add(new DeflateCompressor());
        }

        public DecompressionHandler(params ICompressor[] compressors)
        {
            Guard.ThrowIfNull(compressors);
            this.Compressors = new Collection<ICompressor>();
            foreach (var item in compressors)
            {
                this.Compressors.Add(item);
            }
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken);
        }


        private static async Task<HttpContent> DecompressContentAsync(HttpContent compressedContent, ICompressor compressor)
        {
            using (compressedContent)
            {
                var decompressed = new MemoryStream();
                var contentStream = await compressedContent.ReadAsStreamAsync();
                await compressor.Decompress(contentStream, decompressed).ConfigureAwait(false);

                // set postition back to 0, so it can be read again
                decompressed.Position = 0;
                var newContent = new StreamContent(decompressed);

                // copy content type so we know how to load correct formatter
                newContent.Headers.ContentType = compressedContent.Headers.ContentType;
                return newContent;
            }
        }

    }
}