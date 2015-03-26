using System.Collections.Generic;
using System.Threading.Tasks;
using Tam.Blog.Model;
using Tam.Repository.Contraction;

namespace Tam.Blog.Repository.Interface
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        IList<Post> Posts(int pageNo);

        int TotalPosts();

        IEnumerable<Post> Posts(int pageNo, out int totalRecord);

        IEnumerable<Post> Posts(string searchValue, int pageNo, out int totalRecord);

        //IList<SimplePost> SimplePosts(int pageNo, out int totalRecord);

        Task<Post> GetAndUpdatePost(int Id);

        Post GetAndUpdatePost2(int Id);

        Task<List<Post>> GetNewestItemAsyncs(int take = 12);

        IEnumerable<Post> GetNewestItems(int take = 12);

        IEnumerable<Post> GetItems(int skip = 0, int take = 12);

        IEnumerable<Post> GetItems(out int totalRecords, int skip = 0, int take = 12);

    }
}
