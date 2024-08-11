using System.Linq.Expressions;

namespace DynamicWebApp.Extensions;

//TODO aşağıda kullanılan T için kısıt eklenebilir
public static class IQueryableExtensions
{
    /// <summary>
    /// Used for paging. Can be used as an alternative to Skip(...).Take(...) chaining.
    /// </summary>
    public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int skipCount, int maxResultCount)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (skipCount < 0) skipCount = 0;

        return query.Skip(skipCount).Take(maxResultCount);
    }

    /// <summary>
    /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
    /// </summary>
    /// <param name="query">Queryable to apply filtering</param>
    /// <param name="condition">A boolean value</param>
    /// <param name="predicate">Predicate to filter the query</param>
    /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(query);

        return condition
            ? query.Where(predicate)
            : query;
    }
}
