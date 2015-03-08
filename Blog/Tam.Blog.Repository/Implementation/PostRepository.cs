using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Tam.Blog.Model;
using Tam.Blog.Repository.Interface;
using Tam.Database;

namespace Tam.Blog.Repository.Implementation
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(GreatBlogEntities context, ISqlServerHelper sqlHelper)
            : base(context, sqlHelper)
        {
        }

        //public IList<Post> Posts(int pageNo)
        //{
        //    throw new NotImplementedException();
        //}

        //public int TotalPosts()
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual IList<Post> Posts(int pageNo, out int totalRecord)
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
        //            //post.Tags = this.context.Posts.SingleOrDefault(p => p.Id == post.Id).Tags.ToList();
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
        //    var list = new List<Post>();
        //    return list;
        //}

        //public IList<SimplePost> SimplePosts(int pageNo, out int totalRecord)
        //{
        //    totalRecord = 0;
        //    int pageSize = GetPageSize(PageSize.Post);
        //    var list = new List<SimplePost>();
        //    //try
        //    {
        //        int activePost = Convert.ToInt32(PostStatus.Active);
        //        var items = this.context.Posts.Include("Category").Include("Tags")
        //        .Where(p => p.Status == activePost && p.IsPrivate == false)
        //        .OrderBy(p => p.DataRowVersion)
        //        .Skip(pageNo * pageSize)
        //        .Take(pageSize)
        //        .Select(p => new SimplePost
        //        {
        //            Id = p.Id,
        //            Title = p.Title,
        //            ShortDescription = p.ShortDescription,
        //            Description = p.Description,
        //            Meta = p.Meta,
        //            UrlSlug = p.UrlSlug,
        //            CategoryName = p.Category.Name,
        //            PublishDate = p.PublishDate.Value,
        //            Tags = p.Tags
        //        });
        //        list = items.ToList();
        //    }
        //    //catch (Exception ex)
        //    //{
        //    //    throw;
        //    //}

        //    return list;
        //}

        //private async Task UpdateCountView(Post post)
        //{
        //    post.CountView = ++post.CountView;
        //    this.Update(post);
        //    await Task.FromResult<int>(this.context.SaveChanges());
        //}

        //public async Task<Post> GetAndUpdatePost(int Id)
        //{
        //    // get current post object
        //    var post = base.GetByID(Id);
        //    await UpdateCountView(post);
        //    return post;
        //}

        //public Post GetAndUpdatePost2(int Id)
        //{
        //    var post = base.GetByID(Id);
        //    post.CountView = ++post.CountView;
        //    this.Update(post);
        //    this.context.SaveChanges();
        //    return post;
        //}

        //public Task<ViewListPost> GetPostsAsync(int pageNo, out int totalRecord)
        //{
        //    throw new NotImplementedException();
        //}

        ///// <summary>
        ///// Use this method for Action: Detail, Controller: Blog
        ///// </summary>
        ///// <param name="Id"></param>
        ///// <returns></returns>
        //public async Task<PostDetailViewModel> GetPostDetailAsync(int Id)
        //{
        //    // Solution 1: use list task
        //    var listTask = new List<Task>();
        //    // get list related posts
        //    listTask.Add(this.context.RelatedPosts.Where(p => p.Id == Id).ToListAsync());

        //    // get current post object and increase count view by one
        //    listTask.Add(this.GetAsyncByID(Id));
        //    await Task.WhenAll(listTask);

        //    // Solution 2: sequence await
        //    // get list related posts
        //    //var relatedPosts = await this.context.RelatedPosts.Where(p => p.Id == Id).ToListAsync();

        //    //// get current post object and increase count view by one
        //    //Post post = await this.GetAsyncByID(Id);

        //    List<RelatedPost> relatedPosts = ((Task<List<RelatedPost>>)listTask[0]).Result;
        //    Post post = ((Task<Post>)listTask[1]).Result;
        //    if (post != null)
        //    {
        //        post.CountView = ++post.CountView;
        //        this.Update(post);
        //        await this.context.SaveChangesAsync();
        //    }

        //    // return
        //    return new PostDetailViewModel()
        //    {
        //        Post = post,
        //        ReleatedPosts = relatedPosts
        //    };
        //}

        //private Post GetStaticPost(int Id)
        //{
        //    string fileName = CrytorEngine.Encrypt(Id + "_vX@01");
        //    string filePath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), fileName);
        //    string content = CrytorEngine.Decrypt(File.ReadAllText(filePath));
        //    return null;
        //}

        public Task<Post> GetAndUpdatePost(int Id)
        {
            throw new NotImplementedException();
        }

        public Post GetAndUpdatePost2(int Id)
        {
            throw new NotImplementedException();
        }

        public IList<Post> Posts(int pageNo)
        {
            throw new NotImplementedException();
        }

        public int TotalPosts()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<List<Post>> GetNewestItemAsyncs(int take = 12)
        {
            string storedName = "usp_Blog_GetNewestPost";
            var result = await this.sqlHelper.GetManyItemsAsync<Post>(storedName, new { Take = take });
            return result.ToList();
        }

        public IEnumerable<Post> GetNewestItems(int take = 12)
        {
            string storedName = "usp_Blog_GetNewestPost";
            var result = this.sqlHelper.GetManyItems<Post>(storedName, new { Take = take });
            return result;
        }

        public IEnumerable<Post> Posts(int pageNo, out int totalRecord)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Post> Posts(string searchValue, int pageNo, out int totalRecord)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Post> GetItems(int skip = 0, int take = 12)
        {
            string storedName = "usp_Blog_GetPosts";
            var items = this.sqlHelper.GetManyItems<Post>(storedName, new { Skip = skip, Take = take });
            return items;
        }

        public IEnumerable<Post> GetItems(out int totalRecords, int skip = 0, int take = 12)
        {
            string storedName = "usp_Blog_GetPostsAndTotalRecords";
            var parameters = new DynamicParameters();
            parameters.Add("Skip", skip);
            parameters.Add("Take", take);
            parameters.Add("TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var items = this.sqlHelper.GetManyItems<Post>(storedName, parameters);
            totalRecords = parameters.Get<int>("TotalRecord");
            return items;
        }
    }
}