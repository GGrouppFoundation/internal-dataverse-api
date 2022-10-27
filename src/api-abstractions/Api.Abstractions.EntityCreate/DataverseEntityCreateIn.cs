using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record class DataverseEntityCreateIn<TInJson>
    where TInJson : notnull
{
    public DataverseEntityCreateIn(
        string entityPluralName,
        [AllowNull] FlatArray<string> selectFields,
        TInJson entityData)
    {
        EntityPluralName = entityPluralName ?? string.Empty;
        SelectFields = selectFields ?? Array.Empty<string>();
        EntityData = entityData;
    }

    public string EntityPluralName { get; }

    public FlatArray<string> SelectFields { get; }

    public TInJson EntityData { get; }
}