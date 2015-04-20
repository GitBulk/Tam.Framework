using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Tam.Query
{
    public class QueryProjection<TSource> : IProjection<TSource> where TSource : class
    {
        private DbContext context;

        private DbSet<TSource> dbSet;

        public QueryProjection(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("Context");
            }
            this.context = context;
            this.dbSet = context.Set<TSource>();
        }

        private IQueryable<TSource> Filter(Expression<Func<TSource, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            IQueryable<TSource> query = dbSet;
            query = query.Where(filter);
            return query;
        }

        public List<TDestination> GetItems<TDestination>() where TDestination : class
        {
            IQueryable<TSource> query = dbSet;
            return query.Project().To<TDestination>().ToList();
        }

        public List<TDestination> GetItems<TDestination>(Expression<Func<TSource, bool>> filter) where TDestination : class
        {
            IQueryable<TSource> query = Filter(filter);
            return query.Project().To<TDestination>().ToList();
        }

        public List<TDestination> GetItems<TDestination>(Expression<Func<TSource, bool>> filter, Func<IQueryable<TSource>, IOrderedQueryable<TSource>> order) where TDestination : class
        {
            if (order == null)
            {
                throw new ArgumentNullException("order");
            }
            IQueryable<TSource> query = Filter(filter);
            var result = order(query).Project().To<TDestination>().ToList();
            return result;
        }

        public async Task<List<TDestination>> GetItemsAsync<TDestination>(Expression<Func<TSource, bool>> filter) where TDestination : class
        {
            IQueryable<TSource> query = Filter(filter);
            return await query.Project().To<TDestination>().ToListAsync();
        }

        public async Task<List<TDestination>> GetItemsAsync<TDestination>() where TDestination : class
        {
            IQueryable<TSource> query = dbSet;
            return await query.Project().To<TDestination>().ToListAsync();
        }

        public async Task<List<TDestination>> GetItemsAsync<TDestination>(Expression<Func<TSource, bool>> filter, Func<IQueryable<TSource>, IOrderedQueryable<TSource>> order) where TDestination : class
        {
            if (order == null)
            {
                throw new ArgumentNullException("order");
            }
            IQueryable<TSource> query = Filter(filter);
            List<TDestination> result = await order(query).Project().To<TDestination>().ToListAsync();
            return result;
        }
    }
}
