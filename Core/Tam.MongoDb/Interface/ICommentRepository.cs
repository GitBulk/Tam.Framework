using MongoDB.Bson;
using System;
using System.Collections.Generic;
using Tam.MongoDb.Model;
using Tam.Repository.MongoDb;

namespace Tam.MongoDb.Interface
{
    public interface ICommentRepository : IMongoRepository<Comment>
    {
        //void AddComment(ObjectId postId, Comment comment);
        //List<Comment> GetComments(ObjectId postId, int skip, int take, int totalComments);
        int GetTotalComments(ObjectId postId);
        //void RemoveComment(ObjectId postId, ObjectId commentId);
    }
}
