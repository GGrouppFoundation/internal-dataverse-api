using System;
using System.Collections.Generic;

namespace GGroupp.Infra;

internal static class FlatArrayExtensions
{
    internal static FlatArray<string> FilterNotEmpty(this FlatArray<string> source)
    {
        if (source.IsEmpty)
        {
            return default;
        }

        var list = new List<string>(source.Length);

        for (var i = 0; i < source.Length; i++)
        {
            var item = source[i];

            if (string.IsNullOrEmpty(item))
            {
                continue;
            }

            list.Add(item);
        }

        return list;
    }

    internal static FlatArray<TResult> Map<TSource, TResult>(this FlatArray<TSource> source, Func<TSource, TResult> map)
    {
        if (source.IsEmpty)
        {
            return default;
        }

        var array = new TResult[source.Length];

        for (var i = 0; i < source.Length; i++)
        {
            var item = source[i];
            array[i] = map.Invoke(item);
        }

        return array;
    }
}