using System.Collections.Generic;
using System.Threading.Tasks;
using Tam.Blog.Model;
using Tam.Repository.Contraction;

namespace Tam.Blog.Repository.Interface
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        IList<Post> PostsByCategory(string categorySlug, int pageNo, int pageSize);

        Task<List<Post>> PostsByCategoryAsync(string categorySlug, int pageNo, int pageSize);

        IList<Post> PostsByCategory(string categorySlug, int pageNo);

        IList<Post> PostsByCategory(string categorySlug, int pageNo, out int recordCount);

        /// <summary>
        /// Get posts belongs to a particular category
        /// </summary>
        /// <param name="categoryId">cate id</param>
        /// <param name="pageNo"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        IList<Post> PostsByCategory(int categoryId, int pageNo, out int recordCount);

        int TotalPostByCategory(string categorySlug);

        Category GetCategory(string categorySlug);

    }
}
