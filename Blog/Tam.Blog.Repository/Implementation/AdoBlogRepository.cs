using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tam.Blog.Model;
using Tam.Blog.Repository.Interface;

namespace Tam.Blog.Repository.Implementation
{
    //public class AdoBlogRepository : GenericRepository<Post>, IBlogRepository
    //{
    //    public AdoBlogRepository(GreatBlogEntities context)
    //        : base(context)
    //    {

    //    }

        //public IList<Post> Posts(int pageNo)
        //{
        //    throw new NotImplementedException();
        //}

        //public int TotalPosts()
        //{
        //    throw new NotImplementedException();
        //}

        //public IList<Post> SimplePosts(int pageNo, out int totalRecord)
        //{
        //    totalRecord = 0;
        //    var sqlUtil = new SqlServerUtil(GetSqlConnectionString());
        //    var paramters = new List<SqlParameter>();
        //    paramters.Add(SqlServerUtil.SetSqlParameter("@PageNo", pageNo));
        //    SqlParameter outputParameter = SqlServerUtil.SetSqlParameter("@ResordCount", totalRecord, System.Data.ParameterDirection.Output);
        //    paramters.Add(outputParameter);
        //    string query = "usp_Blog_GetLatestPosts";
        //    SqlDataReader reader = sqlUtil.GetDataReader(query, System.Data.CommandType.StoredProcedure, paramters);
        //    var list = new List<Post>();
        //    if (reader.HasRows)
        //    {
        //        Post post;
        //        while (reader.Read())
        //        {
        //            post = PostFactory.CreatePost(reader);
        //            //
        //            post.Tags = this.context.Posts.SingleOrDefault(p => p.Id == post.Id).Tags.ToList();
        //            list.Add(post);
        //        }
        //        reader.Close();
        //        totalRecord = Convert.ToInt32(outputParameter.Value);
        //    }
        //    return list;
        //}

        //public IList<Post> Posts(string searchValue, int pageNo, out int totalRecord)
        //{
        //    totalRecord = 0;
            //var sqlUtil = new SqlServerUtil(GetSqlConnectionString());
            //var paramters = new List<SqlParameter>();
            //paramters.Add(SqlServerUtil.SetSqlParameter("@SearchValue", pageNo));
            //paramters.Add(SqlServerUtil.SetSqlParameter("@PageNo", pageNo));
            //SqlParameter outputParameter = SqlServerUtil.SetSqlParameter("@ResordCount", totalRecord, System.Data.ParameterDirection.Output);
            //paramters.Add(outputParameter);
            //string query = "usp_Blog_SearchPosts";
            //SqlDataReader reader = sqlUtil.GetDataReader(query, System.Data.CommandType.StoredProcedure, paramters);
            //var list = new List<Post>();
            //if (reader.HasRows)
            //{
            //    Post post;
            //    while (reader.Read())
            //    {
            //        post = PostFactory.CreatePost(reader);
            //        list.Add(post);
            //    }
            //    reader.Close();
            //    totalRecord = Convert.ToInt32(outputParameter.Value);
            //}
        //    return null;
        //}


    //    public IList<Post> Posts(int pageNo)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public int TotalPosts()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IList<Post> Posts(int pageNo, out int totalRecord)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IList<Post> Posts(string searchValue, int pageNo, out int totalRecord)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<Post> GetAndUpdatePost(int Id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Post GetAndUpdatePost2(int Id)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
