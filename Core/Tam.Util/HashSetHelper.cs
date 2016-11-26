using System;
using System.Collections.Generic;
using System.Linq;

namespace Tam.Util
{
    public static class HashSetHelper
    {
        public static HashSet<T> AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                hashSet.Add(item);
            }
            return hashSet;
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> collection)
        {
            return new HashSet<T>(collection);
        }

        public static HashSet<T> FindAll<T>(this HashSet<T> collection, Func<T, bool> codition)
        {
            return new HashSet<T>(collection.Where(codition));
        }
    }
}
