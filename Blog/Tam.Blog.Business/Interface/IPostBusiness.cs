using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tam.Blog.Model;

namespace Tam.Blog.Business.Interface
{
    public interface IPostBusiness : IBaseBusiness<Post>
    {
        List<Post> GetNewestItems(int take = 12);

        Task<List<Post>> GetNewestItemsAsync(int take = 12);

        List<Post> GetNewestItems(int skip = 0, int take = 12);

        List<Post> GetNewestItems(out int totalRecords, int skip = 0, int take = 12);
    }
}
