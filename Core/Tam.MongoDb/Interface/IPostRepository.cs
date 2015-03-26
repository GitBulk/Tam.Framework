using Tam.MongoDb.Model;
using Tam.Repository.MongoDb;

namespace Tam.MongoDb.Interface
{
    public interface IPostRepository<T> : IMongoRepository<Post>
    {
        Post GetPost(string url);
    }
}
