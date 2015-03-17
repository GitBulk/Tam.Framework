using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.MongoDb.Model
{
    public partial class Comment : MongoBaseEntity
    {
        public DateTime CreatedDate { get; set; }

        public string Author { get; set; }

        public string Detail { get; set; }
    }
}
