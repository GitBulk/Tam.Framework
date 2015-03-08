using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tam.Blog.Model;

namespace Tam.Blog.Repository.Interface
{
    public interface ITagRepository : IBaseRepository<Tag>
    {
        IList<Post> PostsByTag(int tagId, int pageNo, out int recordCount);

    }
}
