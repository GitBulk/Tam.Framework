using Microsoft.AspNetCore.Http;
using System;

namespace Tam.NetCore.Util
{
    public static class CookieHelper
    {
        public static void AddCookie(this HttpResponse response, string key, string value, DateTimeOffset? dateExpired)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new Exception("Please set a key for cookie.");
            }
            if (response != null)
            {
                if (dateExpired == null)
                {
                    response.Cookies.Append(key, value);
                }
                else
                {
                    var options = new CookieOptions();
                    options.Expires = dateExpired.Value;
                    response.Cookies.Append(key, value, options);
                }
            }
        }

        public static void AddCookie(this HttpResponse response, string key, string value)
        {
            AddCookie(response, key, value, null);
        }

        public static void DeleteCookie(this HttpResponse response, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new Exception("Please set a key for cookie.");
            }
            if (response != null)
            {
                response.Cookies.Delete(key);
            }
        }

        public static bool GetCookie(this HttpRequest request, string key, out string result)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new Exception("Please set a key for cookie.");
            }
            bool canGet = false;
            result = string.Empty;
            if (request != null)
            {
                canGet = request.Cookies.TryGetValue(key, out result);
            }
            return canGet;
        }
    }
}
