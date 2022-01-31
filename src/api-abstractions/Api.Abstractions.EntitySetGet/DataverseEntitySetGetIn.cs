using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record class DataverseEntitySetGetIn
{
    public DataverseEntitySetGetIn(
        string entityPluralName,
        [AllowNull] IReadOnlyCollection<string> selectFields,
        [AllowNull] string filter,
        int? top = null)
    {
        EntityPluralName = entityPluralName ?? string.Empty;
        SelectFields = selectFields ?? Array.Empty<string>();
        Filter = filter ?? string.Empty;
        Top = top;
    }

    public string EntityPluralName { get; }

    public IReadOnlyCollection<string> SelectFields { get; }

    public string Filter { get; }

    public int? Top { get; }
}