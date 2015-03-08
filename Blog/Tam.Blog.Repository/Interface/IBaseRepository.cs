using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tam.Repository;

namespace Tam.Blog.Repository.Interface
{
    public interface IBaseRepository<T> : ICrudAsyncRepository<T>,
        ICrudRepository<T>, IConnectionStringRepository where T : class
    { }
}