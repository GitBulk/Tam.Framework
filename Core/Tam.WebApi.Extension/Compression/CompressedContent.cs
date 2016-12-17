using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tam.Util;

namespace Tam.WebApi.Extension.Compression
{
    public class CompressedContent : HttpContent
    {
        private readonly ICompressor compressor;
        private readonly HttpContent content;

        public CompressedContent(HttpContent content, ICompressor compressor)
        {
            Guard.ThrowIfNull(content);
            Guard.ThrowIfNull(compressor);
            this.content = content;
            this.compressor = compressor;
            AddHeaders();
        }

        protected virtual void AddHeaders()
        {
            foreach (var header in content.Headers)
            {
                Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            Headers.ContentEncoding.Add(this.compressor.EncodingType);
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            using (this.content)
            {
                var contentStream = await this.content.ReadAsStreamAsync();
                await this.compressor.Compress(contentStream, stream);
            }
        }

        protected override bool TryComputeLength(out long length)
        {
            // length not known at this time
            length = -1;
            return false;
        }
    }
}