using MongoDB.Bson;
using System;
using Tam.Repository.MongoDb;

namespace Tam.MongoDb.Model
{
    public partial class Comment : MongoBaseEntity
    {
        public DateTime CreatedDate { get; set; }

        public string Author { get; set; }

        public string Detail { get; set; }

        public ObjectId PostId { get; set; }
    }
}
