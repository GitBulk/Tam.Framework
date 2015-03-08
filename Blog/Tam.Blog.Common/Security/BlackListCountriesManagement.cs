using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tam.Blog.Common.Security
{
    public class BlackListCountriesManagement
    {
        private static string[] BlackList = { "CN", // Chinese
                                             "HK", // Hong Kong
                                             "MO", // Macau
                                             "TW", // Taiwan
                                             "CHS", // Chinese (Simplified)
                                             "CHT", //	Chinese (Traditional)
                                           };

        public static string[] BlacklistCountries
        {
            get
            {
                if (ConfigurationManager.AppSettings["BlackListCountry"] != null)
                {
                    var temp = ConfigurationManager.AppSettings["BlackListCountry"].ToString();
                    return temp.Split(',');
                }
                return BlackList;
            }
            set
            {
                BlackList = value;
            }
        }

        public static string RedirectUrl = "/Introduce/Index";

        /// <summary>
        /// Check user is allowed access web site or not
        /// </summary>
        /// <param name="request">Current http request</param>
        /// <returns></returns>
        public static bool IsAllowedAccess(HttpRequestBase request)
        {
            var userLanguages = string.Join(",", request.UserLanguages);
            for (int i = 0; i < BlackList.Length; i++)
            {
                if (userLanguages.IndexOf(BlackList[i]) > -1)
                {
                    // user country is in black list
                    return false;
                }
            }
            return true;
        }

        //public static void Filter(ActionExecutingContext filterContext)
        //{
        //    // http://geekswithblogs.net/Aligned/archive/2014/08/12/mvc-onactionexecuting-to-redirect.aspx
        //    // 1. Get user languages and current action, controller.
        //    string userLanguages = string.Join(",", filterContext.RequestContext.HttpContext.Request.UserLanguages);
        //    var descriptor = filterContext.ActionDescriptor;
        //    //string actionName = descriptor.ActionName;
        //    string controllerName = descriptor.ControllerDescriptor.ControllerName;

        //    // 2. Is language of user existing in black list of countries
        //    var result = BlackList.FirstOrDefault(c => userLanguages.IndexOf(c) > -1);
        //    if (string.IsNullOrEmpty(result) == false) // black list contains user languages --> access denied
        //    {
        //        //if (controllerName == "Home")
        //        {
        //            filterContext.Result = new RedirectResult(RedirectUrl);
        //        }
        //    }
        //}
    }
}