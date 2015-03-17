using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tam.MongoDb.Model;

namespace Tam.MongoDb.Interface
{
    public interface IPostRepository<T> : IMongoRepository<T> where T : MongoBaseEntity
    {
        Post GetPost(string url);
    }
}
