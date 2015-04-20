using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Query
{
    public static class QueryableExtension
    {
        public static IProjectionExpression Project<TSource>(this IQueryable<TSource> source) where TSource : class
        {
            return new ProjectionExpression<TSource>(source);
        }

        public static IProjectionExpression Project<TSource>(this IOrderedQueryable<TSource> source) where TSource : class
        {
            return new ProjectionExpression<TSource>(source);
        }
    }
}
