using System.IO;
using System.IO.Compression;

namespace Tam.WebApi.Extension.Compression
{
    public class DeflateCompressor : Compressor
    {
        private const string Name = "deflate";

        public override string EncodingType
        {
            get
            {
                return Name;
            }
        }

        public override Stream CreateCompressionStream(Stream output)
        {
            return new DeflateStream(output, CompressionMode.Compress, true);
        }

        public override Stream CreateDecompressionStream(Stream input)
        {
            return new DeflateStream(input, CompressionMode.Decompress, true);
        }
    }
}
