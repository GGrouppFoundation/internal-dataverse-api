using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public readonly record struct DataverseEntitySetGetOut<TEntityJson>
{
    public DataverseEntitySetGetOut(FlatArray<TEntityJson> value, [AllowNull] string nextLink = null)
    {
        Value = value;
        NextLink = string.IsNullOrEmpty(nextLink) ? null : nextLink;
    }

    public FlatArray<TEntityJson> Value { get; }

    public string? NextLink { get; init; }
}