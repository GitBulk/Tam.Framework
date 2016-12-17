using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Tam.WebApi.Extension.Compression
{
    public abstract class Compressor : BaseCompressor, ICompressor
    {
        public abstract Stream CreateCompressionStream(Stream output);
        public abstract Stream CreateDecompressionStream(Stream input);

        public virtual Task Compress(Stream source, Stream destination)
        {
            var compressed = CreateCompressionStream(destination);
            return CopyToAsync(source, compressed).ContinueWith(task => compressed.Dispose());
        }

        public Task Decompress(Stream source, Stream destination)
        {
            var decompressed = CreateDecompressionStream(source);
            return CopyToAsync(decompressed, destination).ContinueWith(task => decompressed.Dispose());
        }

        protected virtual Task CopyToAsync(Stream input, Stream output)
        {
            return input.CopyToAsync(output);
        }
    }
}