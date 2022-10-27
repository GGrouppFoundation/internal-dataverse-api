using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record class DataverseEntityUpdateIn<TInJson>
    where TInJson : notnull
{
    public DataverseEntityUpdateIn(
        string entityPluralName,
        IDataverseEntityKey entityKey,
        [AllowNull] FlatArray<string> selectFields,
        TInJson entityData)
    {
        EntityPluralName = entityPluralName ?? string.Empty;
        EntityKey = entityKey;
        SelectFields = selectFields ?? FlatArray.Empty<string>();
        EntityData = entityData;
    }

    public string EntityPluralName { get; }

    public FlatArray<string> SelectFields { get; }

    public TInJson EntityData { get; }

    public IDataverseEntityKey EntityKey { get; }
}