using Tam.Compression;

namespace Tam.WebApi.Extension.Compression
{
    public abstract class BaseCompressor : IBaseCompressor
    {
        public abstract string EncodingType
        {
            get;
        }
    }
}