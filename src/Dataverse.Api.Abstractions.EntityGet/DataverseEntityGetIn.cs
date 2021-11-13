using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record class DataverseEntityGetIn
{
    public DataverseEntityGetIn(
        string entityPluralName,
        IDataverseEntityKey entityKey,
        [AllowNull] IReadOnlyCollection<string> selectFields)
    {
        EntityPluralName = entityPluralName ?? string.Empty;
        EntityKey = entityKey;
        SelectFields = selectFields ?? Array.Empty<string>();
    }

    public string EntityPluralName { get; }

    public IDataverseEntityKey EntityKey { get; }

    public IReadOnlyCollection<string> SelectFields { get; }
}