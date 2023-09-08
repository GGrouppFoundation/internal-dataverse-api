using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra;

internal sealed record class DataverseJsonContentOut
{
    public DataverseJsonContentOut([AllowNull] string json)
        =>
        Json = json.OrEmpty();

    public string Json { get; }
}