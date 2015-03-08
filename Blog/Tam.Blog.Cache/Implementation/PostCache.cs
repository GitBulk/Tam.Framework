using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tam.Blog.Cache.Interface;
using Tam.Blog.Model;
using Tam.Blog.Repository.Interface;
//using Tam.Blog.ServiceLayer.Interface;
using Tam.Cache;

namespace Tam.Blog.Cache.Implementation
{
    public class PostCache : BaseCache<Post>, IPostCache
    {
        const string Cache12Items = "TablePost_Cache12NewestItems";
        const string CachePostItem = "PostItem_Id_{0}";
        private ReaderWriterLockSlim CacheLock = new ReaderWriterLockSlim();
        //private static int ExpiredTime = 60;// 60 minute


        //IPostService postService;
        //public PostCache(IPostService postService)
        //{
        //    this.postService = postService;
        //}

        IPostRepository postRepository;
        public PostCache(IPostRepository postRepository)
            : base(postRepository)
        {
            this.postRepository = postRepository;
        }

        private static DataTable ConvertToDataTable(List<Post> posts)
        {
            var table = new DataTable();
            table.Columns.Add("Id");
            table.Columns.Add("Title");
            table.Columns.Add("ShortDescription");
            table.Columns.Add("Description");
            table.Columns.Add("Meta");

            table.Columns.Add("UrlSlug");
            table.Columns.Add("Status");
            table.Columns.Add("CreatedDate");
            table.Columns.Add("UpdatedDate");
            table.Columns.Add("CategoryId");

            table.Columns.Add("PublishDate");
            table.Columns.Add("IsPrivate");
            table.Columns.Add("SearchValue");
            table.Columns.Add("CountView");

            foreach (var item in posts)
            {
                //DataRow row = table.NewRow();
                table.Rows.Add(item.Id, item.Title, item.ShortDescription, item.Description, item.Meta,
                    item.UrlSlug, item.Status, item.CreatedDate, item.UpdatedDate, item.CategoryId,
                    item.PublishDate, item.IsPrivate, item.SearchValue, item.CountView);
            }
            return table;
        }

        public List<Post> Get12NewestItems()
        {
            // https://msdn.microsoft.com/en-us/library/office/ee558270%28v=office.14%29.aspx
            //            var tenItems = LRUCache.Get<List<Post>>(Cache10Item);
            //            lock (LockObject)
            //            {
            //                if (tenItems == null)
            //                {
            //                    int expired = 60; // 60 minute;
            //#if DEBUG
            //                    expired = 5;
            //#endif
            //                    //tenItems = this.postService.Get10NewestItem();
            //                    tenItems = this.postService.Get10NewestItem();
            //                    LRUCache.Set(Cache10Item, tenItems, expired);
            //                }
            //            }
            //            return tenItems;

            // https://msdn.microsoft.com/en-us/library/system.threading.readerwriterlockslim.aspx
            //var tenItems = LRUCache.Get<List<Post>>(Cache10Item);
            //if (tenItems == null)
            //{
            //    CacheLock.EnterWriteLock();
            //    try
            //    {
            //        tenItems = this.postService.Get10NewestItem();
            //        LRUCache.Set(Cache10Item, tenItems, ExpiredTime);
            //    }
            //    finally
            //    {
            //        CacheLock.ExitWriteLock();
            //    }
            //}
            //return tenItems;

            var items = GetNewestItems(12);
            return items;

        }

        //public Post GetItem(object id)
        //{
        //    string key = string.Format(CachePostItem, id.ToString());
        //    Post post = LRUCache.Get<Post>(key);

        //Post post = this.GetItem(id);
        //if (post == null)
        //{
        //    CacheLock.EnterWriteLock();
        //    try
        //    {
        //        post = this.GetItem(id);
        //        LRUCache.Set(string.Format(CachePostItem, id.ToString()), post, ExpiredTime);
        //    }
        //    finally
        //    {
        //        CacheLock.ExitWriteLock();
        //    }
        //}

        //    if (post == null)
        //    {
        //        // use double checked locking
        //        lock (LockObject)
        //        {
        //            post = LRUCache.Get<Post>(key);
        //            if (post == null)
        //            {
        //                post = this.postRepository.GetByID(id);
        //                LRUCache.Set(key, post, ExpiredTime);
        //            }
        //        }
        //    }
        //    return post;
        //}


        public List<Post> GetNewestItems(int take)
        {
            if (take < 1)
            {
                throw new Exception("Take must be > 0");
            }
            string key = string.Format("TablePost_Cache{0}NewestItems", take);
            var items = LRUCache.Get<List<Post>>(key);
            //if (tenItems == null)
            //{
            //    lock (LockObject)
            //    {
            //        tenItems = LRUCache.Get<List<Post>>(key);
            //        if (tenItems == null)
            //        {
            //            //tenItems = this.postService.GetNewestItems();
            //            LRUCache.Add(key, tenItems, ExpiredTime);
            //        }
            //    }
            //}
            return items;
        }

        public List<Post> GetRangeItems(int skip, int take)
        {
            string key = string.Format(RangItemFormat, skip, take);
            var items = LRUCache.Get<List<Post>>(key);
            return items;
        }


        public int CountAllPost()
        {
            throw new NotImplementedException();
        }
    }
}
