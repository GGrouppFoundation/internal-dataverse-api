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

        foreach (var item in source)
        {
            if (string.IsNullOrEmpty(item) is false)
            {
                list.Add(item);
            }
        }

        return list;
    }

    internal static FlatArray<TResult> Map<TSource, TResult>(this FlatArray<TSource> source, Func<TSource, TResult> map)
    {
        if (source.IsEmpty)
        {
            return default;
        }

        var builder = FlatArray<TResult>.Builder.OfLength(source.Length);
        var index = 0;

        foreach (var item in source)
        {
            builder[index] = map.Invoke(item);
            index++;
        }

        return builder.MoveToArray();
    }
}