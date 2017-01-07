using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tam.NetCore.Utilities
{
    public static class Guard
    {
        public static void ThrowIfNull(object obj, string errorMessage = "")
        {
            if (obj == null)
            {
                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = "Parameter obj is null.";
                }
                throw new ArgumentNullException(errorMessage);
            }
        }

        public static void ThrowIfNullOrWhiteSpace(string input, string errorMessage = "")
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = "Parameter input is null or white space.";
                }
                throw new ArgumentNullException(errorMessage);
            }
        }

        public static void ThrowIfNullOrEmpty<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate = null)
        {
            bool result = source.AnyOrNull();
            if (!result)
            {
                throw new Exception("Source is null or empty");
            }
        }
    }
}
