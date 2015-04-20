using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using Tam.Repository.MongoDb;

namespace Tam.MongoDb.Model
{
    [BsonIgnoreExtraElements]
    public partial class Post : MongoBaseEntity
    {
        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string Summary { get; set; }

        public string Details { get; set; }

        public string Author { get; set; }

        public int TotalComments { get; set; }

        //public IList<Comment> Comments { get; set; }
    }
}
