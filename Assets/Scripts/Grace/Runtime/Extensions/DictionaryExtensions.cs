using System;
using System.Collections.Generic;

namespace Grace.Runtime.Extensions;

public static class DictionaryExtensions
{
    public static TValue? FirstOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> source, Func<TKey, bool> predicate)
    {
        foreach (var pair in source)
        {
            if (predicate(pair.Key))
            {
                return pair.Value;
            }
        }

        return default;
    }

    public static Dictionary<TKey, TValue> AddRange<TKey, TValue>(this Dictionary<TKey, TValue>? source, Dictionary<TKey, TValue>? range)
    {
        if (source is null)
        {
            source = new();
        }

        if (range is not null)
        {
            foreach (var item in range)
            {
                source[item.Key] = item.Value;
            }
        }

        return source;
    }

    public static void ForEach<TKey, TValue>(this Dictionary<TKey, TValue> source, Action<KeyValuePair<TKey, TValue>, int> action)
    {
        var index = 0;
        foreach (var pair in source)
        {
            action.Invoke(pair, index++);
        }
    }
}