using System.Collections.Generic;

namespace Grace.Runtime.Extensions;

public static class DictionaryExtensions
{
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
}