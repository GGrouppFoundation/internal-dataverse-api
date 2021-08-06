#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp
{
    public sealed record DataverseEntitiesGetOut<TEntityJson>
    {
        public DataverseEntitiesGetOut([AllowNull] IReadOnlyCollection<TEntityJson> value)
            =>
            Value = value ?? Array.Empty<TEntityJson>();

        public IReadOnlyCollection<TEntityJson> Value { get; }
    }
}