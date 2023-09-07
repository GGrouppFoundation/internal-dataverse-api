using System;

namespace GarageGroup.Infra;

internal static class FlatArrayExtensions
{
    internal static FlatArray<string> FilterNotEmpty(this FlatArray<string> source)
    {
        return source.Filter(IsNotEmpty);

        static bool IsNotEmpty(string value)
            =>
            string.IsNullOrEmpty(value) is false;
    }
}