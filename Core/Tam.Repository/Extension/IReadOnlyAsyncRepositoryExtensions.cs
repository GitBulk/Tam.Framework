using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tam.Repository.Contraction;

namespace Tam.Repository.Extension
{
    public static class IReadOnlyAsyncRepositoryExtensions
    {
        public static Task<IEnumerable<T>> GetAsync<T>(this IReadOnlyAsyncRepository<T> repo,
            Func<IQueryable<T>, IQueryable<T>> query) where T : class
        {

            return null;
        }


    }
}
