using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra;

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