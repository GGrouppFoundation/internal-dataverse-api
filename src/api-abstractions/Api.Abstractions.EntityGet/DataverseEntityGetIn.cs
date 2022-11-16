using System.Collections.Generic;

namespace GGroupp.Infra;

public sealed record class DataverseEntityGetIn
{
    public DataverseEntityGetIn(string entityPluralName, IDataverseEntityKey entityKey, FlatArray<string> selectFields)
    {
        EntityPluralName = entityPluralName ?? string.Empty;
        EntityKey = entityKey;
        SelectFields = selectFields;
    }

    public string EntityPluralName { get; }

    public IDataverseEntityKey EntityKey { get; }

    public FlatArray<string> SelectFields { get; }

    public string? IncludeAnnotations { get; init; }
}