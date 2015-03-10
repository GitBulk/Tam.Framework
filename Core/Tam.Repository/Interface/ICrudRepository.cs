using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Repository.Interface
{
    public interface ICrudRepository<T> : IBaseRepository where T : class
    {
        void Add(T entityToInsert);

        void Delete(object id);

        void Delete(T entityToDelete);

        void Dispose();

        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = "");

        IEnumerable<T> GetAll();

        T GetById(object id);

        T GetById(params object[] keyValues);

        T GetItem(Expression<Func<T, bool>> where);

        IQueryable<T> SearchFor(Expression<Func<T, bool>> where, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        IQueryable<T> SearchFor(Expression<Func<T, bool>> where, int skip, int take, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        void Update(T entityToUpdate);
    }
}
