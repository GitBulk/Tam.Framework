using System.IO;
using System.Threading.Tasks;
using Tam.Compression;

namespace Tam.WebApi.Extension.Compression
{
    public interface ICompressor : IBaseCompressor
    {
        Task Compress(Stream source, Stream destination);

        Task Decompress(Stream source, Stream destination);
    }
}