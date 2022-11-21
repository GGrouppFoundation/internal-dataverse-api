using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record class DataverseEntitySetGetIn
{
    public DataverseEntitySetGetIn(string nextLink)
    {
        NextLink = nextLink ?? string.Empty;
        EntityPluralName = string.Empty;
        Filter = string.Empty;
    }

    public DataverseEntitySetGetIn(
        string entityPluralName,
        FlatArray<string> selectFields,
        [AllowNull] string filter,
        FlatArray<DataverseOrderParameter> orderBy = default,
        int? top = null)
    {
        EntityPluralName = entityPluralName ?? string.Empty;
        SelectFields = selectFields;
        Filter = filter ?? string.Empty;
        OrderBy = orderBy;
        Top = top;
    }

    public string? NextLink { get; }

    public string EntityPluralName { get; }

    public FlatArray<string> SelectFields { get; }

    public string Filter { get; }

    public FlatArray<DataverseOrderParameter> OrderBy { get; }

    public int? Top { get; }

    public int? MaxPageSize { get; init; }

    public string? IncludeAnnotations { get; init; }
}