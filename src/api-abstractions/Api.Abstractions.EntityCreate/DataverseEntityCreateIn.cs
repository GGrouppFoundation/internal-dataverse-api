using System;

namespace GGroupp.Infra;

public sealed record class DataverseEntityCreateIn<TInJson>
    where TInJson : notnull
{
    public DataverseEntityCreateIn(string entityPluralName, FlatArray<string> selectFields, TInJson entityData)
    {
        EntityPluralName = entityPluralName ?? string.Empty;
        SelectFields = selectFields;
        EntityData = entityData;
    }

    public string EntityPluralName { get; }

    public FlatArray<string> SelectFields { get; }

    public TInJson EntityData { get; }

    public bool? SuppressDuplicateDetection { get; init; }
}