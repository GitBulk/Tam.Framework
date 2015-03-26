using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Blog.Business.Interface
{
    public interface IBaseBusiness<T> : IBusiness where T : class
    {
        void Add(T entity);

        void Delete(T entity);

        void Update(T entity);

        IEnumerable<T> GetAll();

        T GetItem(object id);
    }
}
