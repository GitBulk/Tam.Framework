using MongoDB.Bson;
using System;
using System.Collections.Generic;
using Tam.MongoDb.Model;

namespace Tam.MongoDb.Interface
{
    public interface ICommentRepository: IMongoRepository<MongoBaseEntity>
    {
        void AddComment(ObjectId postId, Comment comment);
        IList<Comment> GetComments(ObjectId postId, int skip, int take, int totalComments);
        int GetTotalComments(ObjectId postId);
        void RemoveComment(ObjectId postId, ObjectId commentId);
    }
}
