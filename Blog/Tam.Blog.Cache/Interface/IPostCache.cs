using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tam.Blog.Model;

namespace Tam.Blog.Cache.Interface
{
    public interface IPostCache : IBaseCache<Post>
    {
        List<Post> Get12NewestItems();

        List<Post> GetNewestItems(int take);

        List<Post> GetRangeItems(int skip, int take);

        int CountAllPost();
    }
}
