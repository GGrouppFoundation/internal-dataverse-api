using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record class DataverseEntitySetGetOut<TEntityJson>
{
    public DataverseEntitySetGetOut([AllowNull] FlatArray<TEntityJson> value)
        =>
        Value = value ?? FlatArray.Empty<TEntityJson>();

    public FlatArray<TEntityJson> Value { get; }
}