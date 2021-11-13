using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record class DataverseEntityUpdateIn<TRequestJson>
{
    public DataverseEntityUpdateIn(
        string entityPluralName,
        IDataverseEntityKey entityKey,
        [AllowNull] IReadOnlyCollection<string> selectFields,
        TRequestJson entityData)
    {
        EntityPluralName = entityPluralName ?? string.Empty;
        EntityKey = entityKey;
        SelectFields = selectFields ?? Array.Empty<string>();
        EntityData = entityData;
    }

    public string EntityPluralName { get; }

    public IReadOnlyCollection<string> SelectFields { get; }

    public TRequestJson EntityData { get; }

    public IDataverseEntityKey EntityKey { get; }
}