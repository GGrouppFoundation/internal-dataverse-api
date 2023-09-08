using System;

namespace GarageGroup.Infra;

public interface IDataverseEntityUpdateIn<out TInJson> : IDataverseTransactableIn<TInJson>
    where TInJson : notnull
{
    string EntityPluralName { get; }

    FlatArray<string> SelectFields { get; }

    TInJson EntityData { get; }

    IDataverseEntityKey EntityKey { get; }

    FlatArray<DataverseExpandedField> ExpandFields { get; }

    bool? SuppressDuplicateDetection { get; }

    TInJson? IDataverseTransactableIn<TInJson>.Entity
        =>
        EntityData;
}