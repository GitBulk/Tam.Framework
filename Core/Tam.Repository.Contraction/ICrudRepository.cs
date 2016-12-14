using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Repository.Contraction
{
    public interface ICrudRepository<T> where T : class
    {
        /// <summary>
        /// Add item
        /// </summary>
        /// <param name="item">The entity to add</param>
        int Add(T item);

        /// <summary>
        /// Counts the total items/document in the repository.
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// Counts the items/document which are matching the condition in the repository.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Delete an item/document in collection
        /// </summary>
        /// <param name="id">The value representing the ObjectId of the entity to delete.</param>
        int Delete(object id);

        /// <summary>
        /// Delete an item/document in collection
        /// </summary>
        /// <param name="item">The item to delete</param>
        int Delete(T item);

        void Dispose();

        IEnumerable<T> GetItems(Expression<Func<T, bool>> whereCondition = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = "");

        IEnumerable<T> GetAll();

        T GetById(object id);

        T GetById(params object[] keyValues);

        T GetItem(Expression<Func<T, bool>> where);

        //IQueryable<T> SearchFor(Expression<Func<T, bool>> where, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        //IQueryable<T> SearchFor(Expression<Func<T, bool>> where, int skip, int take, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        SearchResult<T> SearchFor(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        SearchResult<T> SearchFor(Expression<Func<T, bool>> filter, int skip, int take, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        int Update(T item);
    }
}
