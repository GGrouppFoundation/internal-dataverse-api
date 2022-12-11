using System;

namespace GGroupp.Infra;

public sealed record class DataverseEntityUpdateIn<TInJson>
    where TInJson : notnull
{
    public DataverseEntityUpdateIn(
        string entityPluralName,
        IDataverseEntityKey entityKey,
        FlatArray<string> selectFields,
        TInJson entityData)
    {
        EntityPluralName = entityPluralName ?? string.Empty;
        EntityKey = entityKey;
        SelectFields = selectFields;
        EntityData = entityData;
    }

    public string EntityPluralName { get; }

    public FlatArray<string> SelectFields { get; }

    public TInJson EntityData { get; }

    public IDataverseEntityKey EntityKey { get; }
}