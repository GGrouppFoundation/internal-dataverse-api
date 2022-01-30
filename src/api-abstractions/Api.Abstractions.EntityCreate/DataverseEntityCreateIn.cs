using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record class DataverseEntityCreateIn<TInJson>
{
    public DataverseEntityCreateIn(
        string entityPluralName,
        [AllowNull] IReadOnlyCollection<string> selectFields,
        TInJson entityData)
    {
        EntityPluralName = entityPluralName ?? string.Empty;
        SelectFields = selectFields ?? Array.Empty<string>();
        EntityData = entityData;
    }

    public string EntityPluralName { get; }

    public IReadOnlyCollection<string> SelectFields { get; }

    public TInJson EntityData { get; }
}