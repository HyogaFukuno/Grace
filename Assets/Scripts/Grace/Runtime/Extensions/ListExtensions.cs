using System;
using System.Collections.Generic;

namespace Grace.Runtime.Extensions;

public static class ListExtensions
{
    public static int FindIndex<T, TParam>(this List<T> source, TParam param, Func<T, TParam, bool> match) => source.FindIndex(param, 0, source.Count, match);
    
    public static int FindIndex<T, TParam>(this List<T> source, TParam param, int startIndex, int count, Func<T, TParam, bool> match)
    {
        if ((uint)startIndex > (uint)source.Count)
        {
            throw new ArgumentOutOfRangeException();
        }

        if (count < 0 || startIndex > source.Count - count)
        {
            throw new ArgumentOutOfRangeException();
        }
        
        var num = startIndex + count;
        for (var index = startIndex; index < num; ++index)
        {
            if (match(source[index], param))
                return index;
        }
        
        return -1;
    }

    public static T? First<T, TParam>(this List<T> source, TParam param, Func<T, TParam, bool> predicate) where T : struct
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
    
    public static T FirstOrDefault<T, TParam>(this List<T> source, TParam param, Func<T, TParam, bool> predicate) where T : struct
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
    
    public static IEnumerable<T> Where<T, TParam>(this List<T> source, TParam param, Func<T, TParam, bool> predicate)
    {
        foreach (var element in source)
        {
            if (predicate(element, param))
            {
                yield return element;
            }
        }
    }

    public static void ForEach<T>(this List<T> source, Action<T, int> action)
    {
        var index = 0;
        foreach (var element in source)
        {
            action.Invoke(element, index++);
        }
    }

    public static void Prepend<T>(this List<T> source, T element) => source.Insert(0, element);
    
}