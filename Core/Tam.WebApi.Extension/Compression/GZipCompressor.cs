using System.IO;
using System.IO.Compression;

namespace Tam.WebApi.Extension.Compression
{
    public class GZipCompressor : Compressor
    {
        private const string Name = "gzip";

        public override string EncodingType
        {
            get
            {
                return Name;
            }
        }

        public override Stream CreateCompressionStream(Stream output)
        {
            return new GZipStream(output, CompressionMode.Compress, true);
        }

        public override Stream CreateDecompressionStream(Stream input)
        {
            return new GZipStream(input, CompressionMode.Decompress, true);
        }
    }
}