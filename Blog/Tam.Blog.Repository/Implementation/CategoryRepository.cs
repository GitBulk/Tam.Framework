using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tam.Blog.Model;
using Tam.Blog.Repository.Interface;
using Tam.Database;

namespace Tam.Blog.Repository.Implementation
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(GreatBlogEntities context, ISqlServerHelper sqlHelper)
            : base(context, sqlHelper)
        {
        }

        //public int TotalPostByCategory(string categorySlug)
        //{
        //    string query = "usp_Blog_TotalPostsByCategory";
        //    var sqlUtil = new SqlServerUtil(GetSqlConnectionString());
        //    try
        //    {
        //        object value = sqlUtil.ExcuteScalar(query, "", categorySlug);
        //        return Convert.ToInt32(value);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public Category GetCategory(string categorySlug)
        {
            if (string.IsNullOrWhiteSpace(categorySlug))
            {
                categorySlug = "";
            }
            //return this.context.Categories.FirstOrDefault(c => string.Equals(c.UrlSlug, categorySlug, StringComparison.OrdinalIgnoreCase));
            //var cate = this.SearchFor(c => c.UrlSlug.Equals(categorySlug, StringComparison.OrdinalIgnoreCase), null).FirstOrDefault();
            // or
            var cate = this.GetItem(c => c.UrlSlug.Equals(categorySlug, StringComparison.OrdinalIgnoreCase));
            return cate;
        }

        public IList<Post> PostsByCategory(string categorySlug, int pageNo)
        {
            throw new NotImplementedException();
        }

        public IList<Post> PostsByCategory(string categorySlug, int pageNo, int pageSize)
        {
            string storedName = "usp_Blog_GetPostsByCategorySlug";
            int skip = pageNo * pageSize;
            var items = this.sqlHelper.GetManyItems<Post>(storedName, new { CategorySlug = categorySlug, Skip = skip, Take = pageSize });
            return items.ToList();

            //    return this.context.Posts.Where(p => p.Status == 1 && p.IsPrivate == false && p.Category.UrlSlug.Equals(categorySlug))
            //        .OrderByDescending(p => p.UpdatedDate)
            //        .Skip(pageNo * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// Get a list post belongs a particular category
        /// </summary>
        /// <param name="categorySlug"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        //public IList<Post> PostsByCategory(string categorySlug, int pageNo, out int recordCount)
        //{
        //    recordCount = 0;
        //    var sqlUtil = new SqlServerUtil(GetSqlConnectionString());
        //    var paramters = new List<SqlParameter>();
        //    paramters.Add(SqlServerUtil.SetSqlParameter("@CateUrlSlug", categorySlug));
        //    paramters.Add(SqlServerUtil.SetSqlParameter("@PageNo", pageNo));
        //    SqlParameter outputParameter = SqlServerUtil.SetSqlParameter("@ResordCount", recordCount, System.Data.ParameterDirection.Output);
        //    paramters.Add(outputParameter);
        //    string query = "usp_Blog_GetPostsByCategorySlug";
        //    SqlDataReader reader = sqlUtil.GetDataReader(query, System.Data.CommandType.StoredProcedure, paramters);
        //    var list = new List<Post>();
        //    if (reader.HasRows)
        //    {
        //        Post post;
        //        while (reader.Read())
        //        {
        //            post = PostFactory.CreatePost(reader);
        //            list.Add(post);
        //        }
        //        reader.Close();
        //        recordCount = Convert.ToInt32(outputParameter.Value);
        //    }
        //    return list;
        //}

        //public IList<Post> PostsByCategory(int categoryId, int pageNo, out int recordCount)
        //{
        //    recordCount = 0;
        //    var sqlUtil = new SqlServerUtil(GetSqlConnectionString());
        //    var paramters = new List<SqlParameter>();
        //    paramters.Add(SqlServerUtil.SetSqlParameter("@CategoryId", categoryId));
        //    paramters.Add(SqlServerUtil.SetSqlParameter("@PageNo", pageNo));
        //    SqlParameter outputParameter = SqlServerUtil.SetSqlParameter("@ResordCount", recordCount, System.Data.ParameterDirection.Output);
        //    paramters.Add(outputParameter);
        //    string query = "usp_Blog_GetPostsByCategoryId";
        //    SqlDataReader reader = sqlUtil.GetDataReader(query, System.Data.CommandType.StoredProcedure, paramters);
        //    var list = new List<Post>();
        //    if (reader.HasRows)
        //    {
        //        Post post;
        //        while (reader.Read())
        //        {
        //            post = PostFactory.CreatePost(reader);
        //            list.Add(post);
        //        }
        //        reader.Close();
        //        recordCount = Convert.ToInt32(outputParameter.Value);
        //    }
        //    return list;
        //}

        public IList<Post> PostsByCategory(string categorySlug, int pageNo, out int recordCount)
        {
            throw new NotImplementedException();
        }

        public IList<Post> PostsByCategory(int categoryId, int pageNo, out int recordCount)
        {
            throw new NotImplementedException();
        }

        public int TotalPostByCategory(string categorySlug)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Post>> PostsByCategoryAsync(string categorySlug, int pageNo, int pageSize)
        {
            string storedName = "usp_Blog_GetPostsByCategorySlug";
            int skip = pageNo * pageSize;
            var items = await this.sqlHelper.GetManyItemsAsync<Post>(storedName, new { CategorySlug = categorySlug, Skip = skip, Take = pageSize });
            return items.ToList();
        }
    }
}