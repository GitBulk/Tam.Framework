using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tam.Repository;
using Tam.Repository.Interface;

namespace Tam.Blog.Repository.Interface
{
    public interface IBaseRepository<T> : IEntityFrameworkRepository<T>, IConnectionStringRepository where T : class
    { }
}