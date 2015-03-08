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
    public class TagRepository : BaseRepository<Tag>, ITagRepository
    {
        public TagRepository(GreatBlogEntities context)
            : base(context)
        {

        }

        //public IList<Post> PostsByTag(int tagId, int pageNo, out int recordCount)
        //{
        //    recordCount = 0;
        //    var sqlUtil = new SqlServerUtil(GetSqlConnectionString());
        //    var paramters = new List<SqlParameter>();
        //    paramters.Add(SqlServerUtil.SetSqlParameter("@TagId", tagId));
        //    paramters.Add(SqlServerUtil.SetSqlParameter("@PageNo", pageNo));
        //    SqlParameter outputParameter = SqlServerUtil.SetSqlParameter("@ResordCount", recordCount, System.Data.ParameterDirection.Output);
        //    paramters.Add(outputParameter);
        //    string query = "usp_Blog_GetPostsByTagId";
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


        public IList<Post> PostsByTag(int tagId, int pageNo, out int recordCount)
        {
            throw new NotImplementedException();
        }
    }
}
