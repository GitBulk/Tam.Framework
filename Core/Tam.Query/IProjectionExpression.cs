using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tam.Query
{
    public interface IProjectionExpression
    {
        IQueryable<TDestination> To<TDestination>() where TDestination : class;
    }

    public interface IOrderProjectionExpression
    {
        IOrderedQueryable<TDestination> To<TDestination>() where TDestination : class;
    }

    //public class OrderProjectionExpression<TSource> : IOrderProjectionExpression where TSource : class
    //{
    //    private static readonly Dictionary<string, Expression> ExpressionCache = new Dictionary<string, Expression>();
    //    private readonly IOrderedQueryable<TSource> orderSource;

    //    public OrderProjectionExpression(IOrderedQueryable<TSource> orderSource)
    //    {
    //        this.orderSource = orderSource;
    //    }

    //    public IQueryable<TDestination> To<TDestination>() where TDestination : class
    //    {
    //        Expression<Func<TSource, TDestination>> selector = GetCachedExtension<TDestination>() ?? BuildExpression<TDestination>();
    //        return this.orderSource.Select(selector);
    //    }


    //    private static Expression<Func<TSource, TDestination>> GetCachedExtension<TDestination>()
    //    {
    //        string key = GetCacheKey<TDestination>();
    //        return (ExpressionCache.ContainsKey(key) ? ExpressionCache[key] as Expression<Func<TSource, TDestination>> : null);
    //    }

    //    private static string GetCacheKey<TDestination>()
    //    {
    //        return string.Format("{0}_{1}", typeof(TSource).FullName, typeof(TDestination).FullName);
    //    }

    //    private static string[] SplitCamelCase(string input)
    //    {
    //        return Regex.Replace(input, "([A-Z])", "$1", RegexOptions.Compiled).Trim().Split(' ');
    //    }

    //    private static Expression<Func<TSource, TDestination>> BuildExpression<TDestination>()
    //    {
    //        var sourceProperties = typeof(TSource).GetProperties();
    //        var destinationProperties = typeof(TDestination).GetProperties().Where(d => d.CanWrite);
    //        var parameterExpression = Expression.Parameter(typeof(TSource), "src");
    //        var bindings = destinationProperties
    //                       .Select(destProp => BuildBinding(parameterExpression, destProp, sourceProperties))
    //                       .Where(binding => binding != null);
    //        var expression = Expression.Lambda<Func<TSource, TDestination>>(Expression.MemberInit(Expression.New(typeof(TDestination)), bindings), parameterExpression);
    //        string key = GetCacheKey<TDestination>();
    //        ExpressionCache.Add(key, expression);
    //        return expression;
    //    }

    //    private static MemberAssignment BuildBinding(ParameterExpression parameterExpression, System.Reflection.PropertyInfo destinationProperty, IEnumerable<System.Reflection.PropertyInfo> sourceProperties)
    //    {
    //        var sourceProperty = sourceProperties.FirstOrDefault(src => src.Name.Equals(destinationProperty.Name, StringComparison.OrdinalIgnoreCase));
    //        if (sourceProperty != null)
    //        {
    //            return Expression.Bind(destinationProperty, Expression.Property(parameterExpression, sourceProperty));
    //        }
    //        var propertyNames = SplitCamelCase(destinationProperty.Name);
    //        if (propertyNames.Length == 2)
    //        {
    //            sourceProperty = sourceProperties.FirstOrDefault(src => src.Name.Equals(propertyNames[0], StringComparison.OrdinalIgnoreCase));
    //            if (sourceProperty != null)
    //            {
    //                var sourceChildProperty = sourceProperty.PropertyType.GetProperties().FirstOrDefault(src => src.Name.Equals(propertyNames[1], StringComparison.OrdinalIgnoreCase));
    //                if (sourceChildProperty != null)
    //                {
    //                    return Expression.Bind(destinationProperty, Expression.Property(Expression.Property(parameterExpression, sourceProperty), sourceChildProperty));
    //                }
    //            }
    //        }

    //        return null;
    //    }
    //}
}
