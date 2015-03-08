using System;
using System.Web.Caching;
using System.Web.Mvc;
using Tam.Util;

namespace Tam.Blog.Common.Security
{
    public class PreventSpamAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// This stored the time between request (in seconds). Default value is 10
        /// </summary>
        public int DelayRequest = 10;

        /// <summary>
        /// This stored a max number access in a page, Default value is 5
        /// </summary>
        public int MaxAccess = 5;

        /// <summary>
        /// This message will be displayed in case of excessive requests
        /// </summary>
        public string ErrorMessage = "Quá nhanh & Quá nguy hiểm.";

        private const string RedirectUrl = "/Introduce";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // 1. Get user agent. If null or empty --> redirect
            string userAgent = filterContext.RequestContext.HttpContext.Request.UserAgent;
            if (string.IsNullOrWhiteSpace(userAgent))
            {
                filterContext.Result = new RedirectResult(RedirectUrl);
            }
            else
            {
                // 2. Check allow user access
                bool allowed = BlackListCountriesManagement.IsAllowedAccess(filterContext.HttpContext.Request);
                if (allowed == false)
                {
                    filterContext.Result = new RedirectResult(RedirectUrl);
                }
                else
                {
                    // 3. check cache
                    string userKey = WebHelper.GetUserIp() + "_" + userAgent;
                    userKey = CryptorEngine.Hash(userKey);

                    // get current cache
                    var cache = filterContext.HttpContext.Cache;
                    if (cache[userKey] != null)
                    {
                        int numberAccess = Convert.ToInt32(cache[userKey]);
                        if (numberAccess > MaxAccess)
                        {
                            filterContext.Result = new RedirectResult(RedirectUrl);
                        }
                        else
                        {
                            numberAccess++;
                            cache[userKey] = numberAccess;
                        }
                    }
                    else
                    {
                        DateTime expirationDate = DateTime.UtcNow.AddSeconds(DelayRequest);
                        cache.Add(userKey, 1, null, expirationDate, Cache.NoSlidingExpiration,
                            CacheItemPriority.Default, null);
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }// end method
    }
}