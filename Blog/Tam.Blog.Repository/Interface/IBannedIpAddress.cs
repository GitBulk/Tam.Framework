using Tam.Blog.Model;
using Tam.Repository.Contraction;
using Tam.Repository.EntityFramework;

namespace Tam.Blog.Repository.Interface
{
    public interface IBannedIpAddress : IBaseRepository<BannedIpAddress>
    {
        void BannIP(User user);
    }
}
