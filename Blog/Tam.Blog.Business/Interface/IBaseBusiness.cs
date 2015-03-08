using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Blog.Business.Interface
{
    public interface IBaseBusiness<T> : IBusiness where T : class
    {
        int Add(T entity);

        int Delete(T entity);

        int Update(T entity);

        IEnumerable<T> GetAll();

        T GetItem(object id);
    }
}
