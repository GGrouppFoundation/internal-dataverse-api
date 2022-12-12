using System;

namespace GGroupp.Infra;

public sealed record class DataverseEntityGetIn
{
    public DataverseEntityGetIn(
        string entityPluralName,
        IDataverseEntityKey entityKey,
        FlatArray<string> selectFields = default,
        FlatArray<DataverseExpandedField> expandFields = default)
    {
        EntityPluralName = entityPluralName ?? string.Empty;
        EntityKey = entityKey;
        SelectFields = selectFields;
        ExpandFields = expandFields;
    }

    public string EntityPluralName { get; }

    public IDataverseEntityKey EntityKey { get; }

    public FlatArray<string> SelectFields { get; }

    public FlatArray<DataverseExpandedField> ExpandFields { get; }

    public string? IncludeAnnotations { get; init; }
}