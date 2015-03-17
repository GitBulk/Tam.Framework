using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Tam.MongoDb.Model
{
    //public interface IBaseEntity<T>
    //{
    //    [BsonId]
    //    T Id { get; set; }
    //    bool Deleted { get; set; }
    //}

    /// <summary>
    /// Generic MongoDb Entity.
    /// </summary>
    /// <typeparam name="TKey">The type of entity's Id.</typeparam>
    public abstract class MongoBaseEntity<TKey>
    {
        [BsonId]
        public TKey Id { get; set; }

        public bool Deleted { get; set; }
    }

    // At this time, I only support ObjectId type for entity's id
    public abstract class MongoBaseEntity : MongoBaseEntity<ObjectId>
    { }

    //public abstract class MongoStringBaseEntity : MongoBaseEntity<string>
    //{ }
}
