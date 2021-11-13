using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record class DataverseEntitySetGetOut<TEntityJson>
{
    public DataverseEntitySetGetOut([AllowNull] IReadOnlyCollection<TEntityJson> value)
        =>
        Value = value ?? Array.Empty<TEntityJson>();

    public IReadOnlyCollection<TEntityJson> Value { get; }
}