using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tam.MongoDb.Interface;
using Tam.MongoDb.Model;

namespace Tam.MongoDb.Implementation
{
    public class PostRepository : MongoRepository<Post>, IPostRepository<Post>
    {
        public PostRepository()
            : base()
        {

        }

        public override List<Post> Get(System.Linq.Expressions.Expression<Func<Post, bool>> condition = null, Func<IQueryable<Post>, IOrderedQueryable<Post>> orderBy = null)
        {
            return this.Collection.FindAll().SetFields(Fields.Exclude("Comments")).SetSortOrder(SortBy.Descending("Date")).ToList();
        }

        public override Post GetById(MongoDB.Bson.ObjectId id)
        {
            var post = this.Collection.Find(Query.EQ("_id", id)).SetFields(Fields.Slice("Comments", -5)).SingleOrDefault();
            if (post != null)
            {
                post.Comments = post.Comments.OrderByDescending(c => c.CreatedDate).ToList();
            }
            return base.GetById(id);
        }

        public Post GetPost(string url)
        {
            var post = this.Collection.Find(Query.EQ("Url", url)).SetFields(Fields.Slice("Comments", -5)).SingleOrDefault();
            if (post != null)
            {
                post.Comments = post.Comments.OrderByDescending(c => c.CreatedDate).ToList();
            }
            return post;
        }

        public override Post Update(Post item)
        {
            //var query = Query<Post>.EQ(i => i.Id, item.Id);
            //var update = Update<Post>.Set<string>(p => p.Title, item.Title)
            //    .Set<string>(p => p.Url)
            //WriteConcernResult result = this.Collection.Update(query, update);
            //return item;
            Post postFromDb = base.GetById(item.Id);
            postFromDb.Title = item.Title;
            postFromDb.Url = item.Url;
            postFromDb.Summary = item.Summary;
            postFromDb.Details = item.Details;
            base.Update(postFromDb);
            return postFromDb;
        }

        public override Post Add(Post item)
        {
            item.Comments = new List<Comment>();
            return base.Add(item);
        }
    }
}
