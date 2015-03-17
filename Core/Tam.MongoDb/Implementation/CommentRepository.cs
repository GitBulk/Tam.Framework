using MongoDB.Bson;
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
    public class CommentRepository : MongoRepository<Comment>, ICommentRepository
    {
        private readonly PostRepository postRepo;
        public CommentRepository()
        {
            this.postRepo = new PostRepository();
        }

        public CommentRepository(PostRepository postRepo)
        {
            this.postRepo = postRepo;
        }

        //public void AddComment(ObjectId postId, Comment comment)
        //{
        //    this.postRepo.Collection.Update(Query.EQ("_id", postId),
        //        Update<Post>.Push(p => p.Comments, comment).Inc(p => p.TotalComments, 1));
        //}

        //public void RemoveComment(ObjectId postId, ObjectId commentId)
        //{
        //    this.postRepo.Collection.Update(Query.EQ("_id", postId),
        //        Update<Post>.Pull(p => p.Comments, c => c.EQ<ObjectId>(m => m.Id, commentId)).Inc(p => p.TotalComments, -1));
        //}

        //public IList<Comment> GetComments(ObjectId postId, int skip, int take, int totalComments)
        //{
        //    var newComments = GetTotalComments(postId) - totalComments;
        //    skip += newComments;
        //    var post = this.postRepo.Collection.Find(Query.EQ("_id", postId))
        //        .SetFields(Fields.Exclude("CreatedDate", "Title", "Url", "Summary", "Details", "Author", "TotalComments")
        //        .Slice("Comments", -skip, take)).SingleOrDefault();
        //    return post.Comments.OrderByDescending(c => c.CreatedDate).ToList();
        //}

        //public int GetTotalComments(ObjectId postId)
        //{
        //    var post = this.postRepo.Collection.Find(Query.EQ("_id", postId)).SetFields(Fields.Include("TotalComments")).SingleOrDefault();
        //    if (post == null)
        //    {
        //        return 0;
        //    }
        //    return post.TotalComments;
        //}

        public void AddComment(ObjectId postId, Comment comment)
        {
            throw new NotImplementedException();
        }

        public IList<Comment> GetComments(ObjectId postId, int skip, int take, int totalComments)
        {
            throw new NotImplementedException();
        }

        public int GetTotalComments(ObjectId postId)
        {
            throw new NotImplementedException();
        }

        public void RemoveComment(ObjectId postId, ObjectId commentId)
        {
            throw new NotImplementedException();
        }

        public void Add(IEnumerable<MongoBaseEntity> items)
        {
            throw new NotImplementedException();
        }

        public MongoBaseEntity Add(MongoBaseEntity item)
        {
            throw new NotImplementedException();
        }

        public long Count(System.Linq.Expressions.Expression<Func<MongoBaseEntity, bool>> condition)
        {
            throw new NotImplementedException();
        }

        public bool Delete(MongoBaseEntity item)
        {
            throw new NotImplementedException();
        }

        public bool Exists(System.Linq.Expressions.Expression<Func<MongoBaseEntity, bool>> condition)
        {
            throw new NotImplementedException();
        }

        public List<MongoBaseEntity> Get(System.Linq.Expressions.Expression<Func<MongoBaseEntity, bool>> condition = null, Func<IQueryable<MongoBaseEntity>, IOrderedQueryable<MongoBaseEntity>> orderBy = null)
        {
            throw new NotImplementedException();
        }

        public new MongoBaseEntity GetById(ObjectId id)
        {
            throw new NotImplementedException();
        }

        public MongoBaseEntity SetDeletedStatus(MongoBaseEntity item)
        {
            throw new NotImplementedException();
        }

        MongoBaseEntity IMongoRepository<MongoBaseEntity>.SetDeletedStatus(ObjectId id)
        {
            throw new NotImplementedException();
        }

        public MongoBaseEntity Update(MongoBaseEntity item)
        {
            throw new NotImplementedException();
        }

        public new SearchResult SearchFor(MongoDB.Driver.IMongoQuery query, int skip, int take)
        {
            throw new NotImplementedException();
        }

        public SearchResult SearchFor(System.Linq.Expressions.Expression<Func<MongoBaseEntity, bool>> condition, int skip, int take = 12)
        {
            throw new NotImplementedException();
        }

        public List<MongoBaseEntity> GetItems(System.Linq.Expressions.Expression<Func<MongoBaseEntity, bool>> condition, Func<IQueryable<MongoBaseEntity>, IOrderedQueryable<MongoBaseEntity>> orderBy = null)
        {
            throw new NotImplementedException();
        }
    }
}
