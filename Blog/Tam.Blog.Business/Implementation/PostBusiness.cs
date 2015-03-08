using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tam.Blog.Business.Interface;
using Tam.Blog.Cache.Interface;
using Tam.Blog.Model;
using Tam.Blog.Model.EnumCollection;
using Tam.Blog.Repository.Interface;

namespace Tam.Blog.Business.Implementation
{
    public class PostBusiness : BaseBusiness<Post>, IPostBusiness
    {
        private static object LockPost = new object();
        private IPostCache postCache;
        private IPostRepository postRepository;
        //IUnitOfWork unitOfWork;

        //static PostService()
        //{
        //    string temp = ConfigurationManager.AppSettings["CacheTimePostTable"].ToString();
        //    if (string.IsNullOrWhiteSpace(temp) == false)
        //    {
        //        try
        //        {
        //            if (Information.IsNumeric(temp))
        //            {
        //                CacheTimeInMinute = Convert.ToInt32(temp);
        //            }
        //        }
        //        catch { }
        //    }
        //}

        public PostBusiness(IUnitOfWork unitOfWork, IPostRepository postRepository, IPostCache postCache)
            : base(unitOfWork, postRepository, null, postCache)
        {
            this.postRepository = postRepository;
            this.postCache = postCache;
            //this.unitOfWork = unitOfWork;
        }

        public List<Post> GetNewestItems(int take = 12)
        {
            //var query = this.postRepository.SearchForAsync(null, 0, 10, orderBy: m => m.OrderByDescending(p => p.DataRowVersion));
            //return query.Result;

            //var posts = this.postRepository.GetNewestItems(take);
            //return posts.ToList();

            //var posts = this.postCache.GetNewestItems(take);
            //if (posts == null)
            //{
            //    posts = this.postRepository.GetNewestItems(take).ToList();
            //    lock (LockPost)
            //    {
            //        var cachePosts = this.postCache.GetNewestItems(take);
            //        if (cachePosts == null)
            //        {
            //            foreach (var item in posts)
            //            {
            //                this.postCache.Add(item.Id.ToString(), item, CacheTimeInMinute);
            //            }
            //        }
            //    }
            //}
            //return posts;

            var posts = GetNewestItems(0, 12);
            return posts;
        }

        public List<Post> GetNewestItems(int skip = 0, int take = 12)
        {
            int totalRecords = 0; // just bypass method
            var posts = GetNewestItems(out totalRecords, skip, take);
            return posts;
            //if (skip < 0)
            //{
            //    throw new Exception("Skip must be >= 0.");
            //}
            //if (take < 1)
            //{
            //    throw new Exception("Take must be > 0.");
            //}
            //var posts = this.postCache.GetRangeItems(skip, take);
            //if (posts == null)
            //{
            //    posts = this.postRepository.GetItems(skip, take).ToList();
            //    lock (LockPost)
            //    {
            //        var cachePosts = this.postCache.GetRangeItems(skip, take);
            //        if (cachePosts == null)
            //        {
            //            foreach (var item in posts)
            //            {
            //                this.postCache.Add(item.Id.ToString(), item, CacheTimeInMinute);
            //            }
            //        }
            //    }
            //}
            //return posts;

            //var posts = this.postRepository.GetItems(skip, take);
            //return posts.ToList();
        }

        public List<Post> GetNewestItems(out int totalRecords, int skip = 0, int take = 12)
        {
            totalRecords = 0;
            //var posts = GetNewestItems(skip, take);
            //totalRecords = postCache.CountAllPost();
            //return posts;
            //var posts = this.postRepository.GetItems(out totalRecords, skip, take);
            //return posts.ToList();

            if (skip < 0)
            {
                throw new Exception("Skip must be >= 0.");
            }
            if (take < 1)
            {
                throw new Exception("Take must be > 0.");
            }
            totalRecords = postCache.CountAllPost();
            var posts = this.postCache.GetRangeItems(skip, take);
            if (posts == null || totalRecords == 0)
            {
                posts = this.postRepository.GetItems(out totalRecords, skip, take).ToList();
                lock (LockPost)
                {
                    var cachePosts = this.postCache.GetRangeItems(skip, take);
                    if (cachePosts == null)
                    {
                        foreach (var item in posts)
                        {
                            this.postCache.Add(item.Id.ToString(), item, CacheTimeInMinute);
                        }
                    }
                }
            }
            return posts;
        }

        public async Task<List<Post>> GetNewestItemsAsync(int take = 12)
        {
            return await this.postRepository.GetNewestItemAsyncs(take);
        }

        public override int Update(Post entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity");
            }
            this.postRepository.Update(entity);
            int result = this.unitOfWork.SaveChanges();
            if (result > 0)
            {
                this.postCache.Update(entity.Id.ToString(), entity, CacheTimeInMinute);
            }
            return result;
            //return base.Update(entity);
        }

        public override int Delete(Post entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity");
            }
            entity.Status = (int)PostStatus.InActive;
            //this.postRepository.Delete(entity);
            this.postRepository.Update(entity);
            int result = this.unitOfWork.SaveChanges();
            if (result > 0)
            {
                this.postCache.Delete(entity.Id.ToString());
            }
            return result;
            //return base.Delete(entity);
        }

        public override int Add(Post entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity");
            }
            //this.baseRepository.Add(entity);
            this.postRepository.Add(entity);
            int result = this.unitOfWork.SaveChanges();
            if (result > 0)
            {
                this.postCache.Add(entity.Id.ToString(), entity, CacheTimeInMinute);
            }
            return result;
            //return base.Add(entity);
        }
    }
}