using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Tam.Blog.Model;


namespace Tam.Blog.Repository.Implementation
{
    //public class CachedBlogRepository : PostRepository
    //{
        //private static readonly object LockObject = new object();

        //public CachedBlogRepository(GreatBlogEntities context)
        //    : base(context)
        //{ }

        //public override IList<Post> Posts(int pageNo, out int totalRecord)
        //{
        //    totalRecord = 0;
        //    string cacheKey = "LatestPosts_" + pageNo;
        //    var result = HttpRuntime.Cache[cacheKey] as List<Post>;
        //    if (result == null)
        //    {
        //        lock (LockObject)
        //        {
        //            result = HttpRuntime.Cache[cacheKey] as List<Post>;
        //            if (result == null)
        //            {
        //                result = base.Posts(pageNo, out totalRecord).ToList();
        //                HttpRuntime.Cache.Insert(cacheKey, result, null, DateTime.UtcNow.AddHours(3), TimeSpan.Zero);
        //            }
        //        }
        //    }
        //    return result;
        //}

        //public bool RemoveCacche(string cacheName)
        //{
        //    try
        //    {
        //        var cache = HttpRuntime.Cache[cacheName];
        //        if (cache != null)
        //        {
        //            HttpRuntime.Cache.Remove(cacheName);
        //        }
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
    //}
}
