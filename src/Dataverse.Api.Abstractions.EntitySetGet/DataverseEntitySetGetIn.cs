using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record DataverseEntitySetGetIn
{
    public DataverseEntitySetGetIn(
        [AllowNull] string entitySetName,
        [AllowNull] IReadOnlyCollection<string> selectFields,
        [AllowNull] string filter,
        int? top = null)
    {
        EntitySetName = entitySetName ?? string.Empty;
        SelectFields = selectFields ?? Array.Empty<string>();
        Filter = filter ?? string.Empty;
        Top = top;
    }

    public string EntitySetName { get; }

    public IReadOnlyCollection<string> SelectFields { get; }

    public string Filter { get; }

    public int? Top { get; }
}