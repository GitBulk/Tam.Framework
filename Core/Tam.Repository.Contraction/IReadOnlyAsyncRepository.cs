using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tam.Repository.Contraction
{
    public interface IReadOnlyAsyncRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync(Func<IQueryable<T>, IQueryable<T>> query, CancellationToken cancellationToken);

        Task<TResult> GetAsync<TResult>(Func<IQueryable<T>, TResult> query, CancellationToken cancellationToken);
    }
}
