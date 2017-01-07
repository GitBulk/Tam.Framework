using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Tam.NetCore.Utilities
{
    public static class LinqExtension
    {
        public static bool AnyOrNull<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate = null)
        {
            if (source == null)
            {
                return false;
            }
            if (predicate == null)
            {
                return source.Any();
            }
            return source.Any(predicate);
        }

        public static bool AnyOrNull<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = null)
        {
            if (source == null)
            {
                return false;
            }
            if (predicate == null)
            {
                return source.Any();
            }
            return source.Any(predicate);
        }
    }
}

