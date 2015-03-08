using System;
using System.Collections.Generic;
using System.Linq;

namespace Tam.Util
{
    public static class DateTimeHelper
    {
        public static List<DateTime> SortAscending(List<DateTime> list)
        {
            var temp = list.ToList();
            temp.Sort((a, b) => a.CompareTo(b));
            return temp;
        }

        public static List<DateTime> SortDescending(List<DateTime> list)
        {
            var temp = list.ToList();
            temp.Sort((a, b) => b.CompareTo(a));
            return temp;
        }

        public static List<DateTime> SortMonthAscending(List<DateTime> list)
        {
            var temp = list.ToList();
            temp.Sort((a, b) => a.Month.CompareTo(b.Month));
            return temp;
        }

        public static List<DateTime> SortMonthDescending(List<DateTime> list)
        {
            var temp = list.ToList();
            temp.Sort((a, b) => b.Month.CompareTo(a.Month));
            return temp;
        }
    }
}