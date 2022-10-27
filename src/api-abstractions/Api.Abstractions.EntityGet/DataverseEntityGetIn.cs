using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record class DataverseEntityGetIn
{
    public DataverseEntityGetIn(
        string entityPluralName,
        IDataverseEntityKey entityKey,
        [AllowNull] FlatArray<string> selectFields)
    {
        EntityPluralName = entityPluralName ?? string.Empty;
        EntityKey = entityKey;
        SelectFields = selectFields ?? Array.Empty<string>();
    }

    public string EntityPluralName { get; }

    public IDataverseEntityKey EntityKey { get; }

    public FlatArray<string> SelectFields { get; }

    public string? IncludeAnnotations { get; init; }
}