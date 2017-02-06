using Tam.Repository.Contraction;

namespace Tam.Repository.MongoDb
{
    public interface IMongoRepository<T> : IBaseRepository<T> where T : MongoBaseEntity
    {
    }
}

