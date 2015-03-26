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

        public void AddComment(ObjectId postId, Comment comment)
        {
            this.postRepo.Collection.Update(Query.EQ("_id", postId),
                Update<Post>.Push(p => p.Comments, comment).Inc(p => p.TotalComments, 1));
        }

        public List<Comment> GetComments(ObjectId postId, int skip, int take, int totalComments)
        {
            var newComments = GetTotalComments(postId) - totalComments;
            skip += newComments;
            var post = this.postRepo.Collection.Find(Query.EQ("_id", postId))
                .SetFields(Fields.Exclude("CreatedDate", "Title", "Url", "Summary", "Details", "Author", "TotalComments")
                .Slice("Comments", -skip, take)).SingleOrDefault();
            return post.Comments.OrderByDescending(c => c.CreatedDate).ToList();
        }

        public int GetTotalComments(ObjectId postId)
        {
            var post = this.postRepo.Collection.Find(Query.EQ("_id", postId)).SetFields(Fields.Include("TotalComments")).SingleOrDefault();
            if (post == null)
            {
                return 0;
            }
            return post.TotalComments;
        }

        public void RemoveComment(ObjectId postId, ObjectId commentId)
        {
            this.postRepo.Collection.Update(Query.EQ("_id", postId),
                Update<Post>.Pull(p => p.Comments, c => c.EQ<ObjectId>(m => m.Id, commentId)).Inc(p => p.TotalComments, -1));
        }
    }
}
