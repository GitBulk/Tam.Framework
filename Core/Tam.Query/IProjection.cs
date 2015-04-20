using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Query
{
    //public interface IProjection
    //{
    //    List<TDestination> GetItems<TDestination>() where TDestination : class;

    //    List<TDestination> GetItems<TSource, TDestination>(Expression<Func<TSource, bool>> filter)
    //        where TSource : class
    //        where TDestination : class;
    //}

    public interface IProjection<TSource> where TSource : class
    {
        List<TDestination> GetItems<TDestination>() where TDestination : class;

        List<TDestination> GetItems<TDestination>(Expression<Func<TSource, bool>> filter,
            Func<IQueryable<TSource>, IOrderedQueryable<TSource>> order) where TDestination : class;

        List<TDestination> GetItems<TDestination>(Expression<Func<TSource, bool>> filter)
            where TDestination : class;

        Task<List<TDestination>> GetItemsAsync<TDestination>() where TDestination : class;

        Task<List<TDestination>> GetItemsAsync<TDestination>(Expression<Func<TSource, bool>> filter)
            //where TSource : class
            where TDestination : class;

        Task<List<TDestination>> GetItemsAsync<TDestination>(Expression<Func<TSource, bool>> filter,
            Func<IQueryable<TSource>, IOrderedQueryable<TSource>> order)
            where TDestination : class;

    }
}
