using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Repository.Interface
{
    public interface IMongoDbRepository<T> : ICrudAsyncRepository<T>,
        ICrudRepository<T> where T : class
    {
    }
}
