using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record DataverseEntityGetIn
{
    public DataverseEntityGetIn(
        [AllowNull] string entityPluralName,
        IDataverseEntityKey entityKey,
        [AllowNull] IReadOnlyCollection<string> selectFields)
    {
        EntityPluralName = entityPluralName ?? string.Empty;
        SelectFields = selectFields ?? Array.Empty<string>();
        EntityKey = entityKey;
    }

    public string EntityPluralName { get; }

    public IReadOnlyCollection<string> SelectFields { get; }

    public IDataverseEntityKey EntityKey { get; }
}