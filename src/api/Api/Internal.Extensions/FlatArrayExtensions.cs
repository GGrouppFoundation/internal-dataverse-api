using System.Collections.Generic;

namespace GGroupp.Infra;

internal static class FlatArrayExtensions
{
    internal static IEnumerable<string> NotEmpty(this FlatArray<string> source)
    {
        foreach (var item in source)
        {
            if (string.IsNullOrEmpty(item) is false)
            {
                yield return item;
            }
        }
    }
}