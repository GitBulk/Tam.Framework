using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tam.MongoDb.Interface;
using Tam.MongoDb.Model;
using Tam.Repository.MongoDb;

namespace Tam.MongoDb.Implementation
{
    public class PostRepository : MongoRepository<Post>, IPostRepository<Post>
    {
        public PostRepository()
            : base()
        {

        }


        public override IEnumerable<Post> GetItems(System.Linq.Expressions.Expression<Func<Post, bool>> filter = null, Func<IQueryable<Post>, IOrderedQueryable<Post>> orderBy = null, string includeProperties = "")
        {
            return this.Collection.FindAll().SetFields(Fields.Exclude("Comments")).SetSortOrder(SortBy.Descending("Date")).ToList();
        }

        public override Post GetById(object id)
        {
            BsonValue value = ConvertToMongoObjectId(id);
            var post = this.Collection.Find(Query.EQ("_id", value)).SetFields(Fields.Slice("Comments", -5)).SingleOrDefault();
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

        public override void Update(Post item)
        {
            Post postFromDb = base.GetById(item.Id);
            postFromDb.Title = item.Title;
            postFromDb.Url = item.Url;
            postFromDb.Summary = item.Summary;
            postFromDb.Details = item.Details;
            base.Update(postFromDb);
        }

        public override void Add(Post item)
        {
            if (item.Comments == null)
            {
                item.Comments = new List<Comment>();
            }
            base.Add(item);
        }
    }
}
