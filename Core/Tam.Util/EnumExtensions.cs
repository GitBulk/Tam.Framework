using System;
using System.Collections.Generic;
using System.Linq;


namespace Tam.Util
{
    public static class EnumExtensions
    {
        public static Array GetArray<T>()
        {
            var arr = Enum.GetValues(typeof(T));
            return arr;
        }
        public static List<T> GetList<T>()
        {
            var arr = GetArray<T>();
            return arr.OfType<T>().ToList();
        }

        public static bool IsDefined<T>(object value)
        {
            return Enum.IsDefined(typeof(T), value);
        }
    }
}
