using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Repository
{
    public interface ICrudAsyncRepository<T> where T : class
    {
        Task AddAsync(T entity);

        Task<int> CountAsync();

        Task<int> CountAsync(System.Linq.Expressions.Expression<Func<T, bool>> match);

        Task<List<T>> GetAllAsync();

        Task<T> GetByIdAsync(object id);

        Task<T> GetItemAsync(Expression<Func<T, bool>> match);

        Task<List<T>> GetItemsAsync(Expression<Func<T, bool>> match);
        Task RemoveAsync(T entity);

        Task<List<T>> SearchForAsync(Expression<Func<T, bool>> where, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        Task<List<T>> SearchForAsync(Expression<Func<T, bool>> where, int skip, int take, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        Task UpdateAsync(T entity);
    }
}
