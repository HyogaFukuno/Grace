using System;
using System.Collections.Generic;

namespace Grace.Runtime.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> Where<T, TParam>(this IEnumerable<T> source, TParam param, Func<T, TParam, bool> predicate)
    {
        foreach (var element in source)
        {
            if (predicate(element, param))
            {
                yield return element;
            }
        }
    }

    public static T? First<T, TParam>(this IEnumerable<T> source, TParam param, Func<T, TParam, bool> predicate) where T : struct
    {
        foreach (var element in source)
        {
            if (predicate(element, param))
            {
                return element;
            }
        }

        return null;
    }
    
    public static T FirstOrDefault<T, TParam>(this IEnumerable<T> source, TParam param, Func<T, TParam, bool> predicate) where T : struct
    {
        foreach (var element in source)
        {
            if (predicate(element, param))
            {
                return element;
            }
        }

        return default;
    }
}