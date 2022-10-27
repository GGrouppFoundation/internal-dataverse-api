using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record class DataverseEntitySetGetIn
{
    public DataverseEntitySetGetIn(
        string entityPluralName,
        [AllowNull] FlatArray<string> selectFields,
        [AllowNull] string filter,
        [AllowNull] FlatArray<DataverseOrderParameter> orderBy = null,
        int? top = null)
    {
        EntityPluralName = entityPluralName ?? string.Empty;
        SelectFields = selectFields ?? FlatArray.Empty<string>();
        Filter = filter ?? string.Empty;
        OrderBy = orderBy ?? FlatArray.Empty<DataverseOrderParameter>();
        Top = top;
    }

    public string EntityPluralName { get; }

    public FlatArray<string> SelectFields { get; }

    public string Filter { get; }

    public FlatArray<DataverseOrderParameter> OrderBy { get; }

    public int? Top { get; }

    public string? IncludeAnnotations { get; init; }
}