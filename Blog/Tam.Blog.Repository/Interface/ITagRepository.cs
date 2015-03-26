using System.Collections.Generic;
using Tam.Blog.Model;
using Tam.Repository.Contraction;

namespace Tam.Blog.Repository.Interface
{
    public interface ITagRepository : IBaseRepository<Tag>
    {
        IList<Post> PostsByTag(int tagId, int pageNo, out int recordCount);

    }
}
