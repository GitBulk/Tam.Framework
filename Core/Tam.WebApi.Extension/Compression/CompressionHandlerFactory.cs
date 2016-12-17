using System.Net.Http;

namespace Tam.WebApi.Extension.Compression
{
    public static class CompressionHandlerFactory
    {
        public static DelegatingHandler CreateCompressionHandler()
        {
            return new CompressionHandler();
        }

        public static DelegatingHandler CreateCompressionHandler(params ICompressor[] compressors)
        {
            return new CompressionHandler(compressors);
        }

        public static DelegatingHandler CreateDecompressionHandler()
        {
            return new DecompressionHandler();
        }

        public static DelegatingHandler CreateDecompressionHandler(params ICompressor[] compressors)
        {
            return new DecompressionHandler(compressors);
        }
    }
}