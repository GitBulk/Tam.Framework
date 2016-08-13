using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        /// <summary>
        /// Returns a unix Epoch time given a Date
        /// </summary>
        public static long ToJavascriptTime(this DateTime dt)
        {
            return (long)(dt - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
        }

        /// <summary>
        /// Converts to Date given an Epoch time
        /// </summary>
        public static DateTime ToDateTime(this long epoch)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(epoch);
        }

        /// <summary>
        /// Returns a humanized string indicating how long ago something happened, eg "3 days ago".
        /// For future dates, returns when this DateTime will occur from DateTime.UtcNow.
        /// </summary>
        public static string ToRelativeTime(this DateTime dt)
        {
            DateTime utcNow = DateTime.UtcNow;
            return dt <= utcNow ? ToRelativeTimePast(dt, utcNow) : ToRelativeTimeFuture(dt, utcNow);
        }

        /// <summary>
        /// Returns a humanized string indicating how long ago something happened, eg "3 days ago".
        /// For future dates, returns when this DateTime will occur from DateTime.UtcNow.
        /// If this DateTime is null, returns empty string.
        /// </summary>
        public static string ToRelativeTime(this DateTime? dt)
        {
            if (dt == null) return "";
            return ToRelativeTime(dt.Value);
        }

        private static string ToRelativeTimePast(DateTime dt, DateTime utcNow)
        {
            TimeSpan ts = utcNow - dt;
            double delta = ts.TotalSeconds;

            if (delta < 60)
            {
                return ts.Seconds == 1 ? "1 sec ago" : ts.Seconds + " secs ago";
            }
            if (delta < 3600) // 60 mins * 60 sec
            {
                return ts.Minutes == 1 ? "1 min ago" : ts.Minutes + " mins ago";
            }
            if (delta < 86400) // 24 hrs * 60 mins * 60 sec
            {
                return ts.Hours == 1 ? "1 hour ago" : ts.Hours + " hours ago";
            }

            int days = ts.Days;
            if (days == 1)
            {
                return "yesterday";
            }
            else if (days <= 2)
            {
                return days + " days ago";
            }
            else if (utcNow.Year == dt.Year)
            {
                return dt.ToString("MMM %d 'at' %H:mmm");
            }
            return dt.ToString(@"MMM %d \'yy 'at' %H:mmm");
        }

        private static string ToRelativeTimeFuture(DateTime dt, DateTime utcNow)
        {
            TimeSpan ts = dt - utcNow;
            double delta = ts.TotalSeconds;

            if (delta < 60)
            {
                return ts.Seconds == 1 ? "in 1 second" : "in " + ts.Seconds + " seconds";
            }
            if (delta < 3600) // 60 mins * 60 sec
            {
                return ts.Minutes == 1 ? "in 1 minute" : "in " + ts.Minutes + " minutes";
            }
            if (delta < 86400) // 24 hrs * 60 mins * 60 sec
            {
                return ts.Hours == 1 ? "in 1 hour" : "in " + ts.Hours + " hours";
            }

            // use our own rounding so we can round the correct direction for future
            var days = (int)Math.Round(ts.TotalDays, 0);
            if (days == 1)
            {
                return "tomorrow";
            }
            else if (days <= 10)
            {
                return "in " + days + " day" + (days > 1 ? "s" : "");
            }
            // if the date is in the future enough to be in a different year, display the year
            if (utcNow.Year != dt.Year)
                return "on " + dt.ToString(@"MMM %d \'yy 'at' %H:mmm");
            else
                return "on " + dt.ToString("MMM %d 'at' %H:mmm");
        }

        /// <summary>
        /// returns a html span element with relative time elapsed since this event occurred, eg, "3 months ago" or "yesterday";
        /// assumes time is *already* stored in UTC format!
        /// </summary>
        public static string ToRelativeTimeSpan(this DateTime dt)
        {
            return ToRelativeTimeSpan(dt, "relativetime");
        }

        public static string ToRelativeTimeSpan(this DateTime dt, string cssclass)
        {
            if (cssclass == null)
                return string.Format(@"<span title=""{0:u}"">{1}</span>", dt, ToRelativeTime(dt));
            else
                return string.Format(@"<span title=""{0:u}"" class=""{2}"">{1}</span>", dt, ToRelativeTime(dt), cssclass);
        }

        public static string ToRelativeTimeSpan(this DateTime? dt)
        {
            if (dt == null) return "";
            return ToRelativeTimeSpan(dt.Value);
        }


        /// <summary>
        /// returns a very *small* humanized string indicating how long ago something happened, eg "3d ago"
        /// </summary>
        public static string ToRelativeTimeMini(this DateTime dt)
        {
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dt.Ticks);
            double delta = ts.TotalSeconds;

            if (delta < 60)
            {
                return ts.Seconds + "s ago";
            }
            if (delta < 3600) // 60 mins * 60 sec
            {
                return ts.Minutes + "m ago";
            }
            if (delta < 86400) // 24 hrs * 60 mins * 60 sec
            {
                return ts.Hours + "h ago";
            }
            int days = ts.Days;
            if (days <= 2)
            {
                return days + "d ago";
            }
            else if (days <= 330)
            {
                return dt.ToString("MMM %d 'at' %H:mmm").ToLowerInvariant();
            }
            return dt.ToString(@"MMM %d \'yy 'at' %H:mmm").ToLowerInvariant();
        }


        public static string ToRelativeTimeMicro(this DateTime dt)
        {
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dt.Ticks);

            if (ts.Days <= 330)
            {
                return dt.ToString("MMM %d").ToLower();
            }
            else
            {
                return dt.ToString("MMM %d yy").ToLower();
            }
        }

        /// <summary>
        /// returns AN HTML SPAN ELEMENT with minified relative time elapsed since this event occurred, eg, "3mo ago" or "yday";
        /// assumes time is *already* stored in UTC format!
        /// </summary>
        public static string ToRelativeTimeSpanMini(this DateTime dt)
        {
            return string.Format(@"<span title=""{0:u}"" class=""relativetime"">{1}</span>", dt, ToRelativeTimeMini(dt));
        }

        /// <summary>
        /// returns AN HTML SPAN ELEMENT with minified relative time elapsed since this event occurred, eg, "3mo ago" or "yday";
        /// assumes time is *already* stored in UTC format!
        /// If this DateTime? is null, will return empty string.
        /// </summary>
        public static string ToRelativeTimeSpanMini(this DateTime? dt)
        {
            if (dt == null) return "";
            return ToRelativeTimeSpanMini(dt.Value);
        }

        public static IHtmlString AsHtml(this string html)
        {
            return MvcHtmlString.Create(html);
        }

        public static IHtmlString ToRelativeTimeSpanMicro(this DateTime dt)
        {
            return string.Format(@"<span title=""{0:u}"" class=""relativetime"">{1}</span>", dt, ToRelativeTimeMicro(dt)).AsHtml();
        }

        public static IHtmlString ToRelativeTimeSpanMicro(this DateTime? dt)
        {
            if (dt == null) return "".AsHtml();
            return ToRelativeTimeSpanMicro(dt.Value);
        }


        public static string ToAtomFeedDate(this DateTime dt)
        {
            return string.Format("{0:yyyy-MM-ddTHH:mm:ssZ}", dt);
        }

        public static string ToAtomFeedDate(this DateTime? dt)
        {
            return dt == null ? "" : ToAtomFeedDate(dt.Value);
        }


        /// <summary>
        /// returns how long something took in sec, minutes, hours, or days
        /// </summary>
        public static string TimeTaken(this TimeSpan time)
        {
            string output = "";
            if (time.Days > 0)
                output += time.Days + " day" + (time.Days > 1 ? "s " : " ");
            if ((time.Days == 0 || time.Days == 1) && time.Hours > 0)
                output += time.Hours + " hour" + (time.Hours > 1 ? "s " : " ");
            if (time.Days == 0 && time.Minutes > 0)
                output += time.Minutes + " minute" + (time.Minutes > 1 ? "s " : " ");
            if (output.Length == 0)
                output += time.Seconds + " second" + (time.Seconds > 1 ? "s " : " ");
            return output.Trim();
        }

        /// <summary>
        /// returns how long something took in years, months, or days
        /// </summary>
        public static string TimeTakenLong(this DateTime dt)
        {
            int days = (DateTime.UtcNow - dt).Days;
            if (days <= 0)
                return "today";
            if (days <= 1)
                return "yesterday";
            if (days > 365)
            {
                return (days / 365) + " year" + ((days / 365) > 1 ? "s ago" : " ago");
            }
            if (days > 30)
            {
                return (days / 30) + " month" + ((days / 30) > 1 ? "s ago" : " ago");
            }
            return days + " day" + (days > 1 ? "s ago" : " ago");
        }

        /// <summary>
        /// returns how long something took in years, months, or days
        /// </summary>
        public static string TimeTakenLong(this DateTime? dt)
        {
            if (dt == null) return "";
            return TimeTakenLong(dt.Value);
        }
    }
}