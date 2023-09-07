using System;

namespace GarageGroup.Infra;

public interface IDataverseEntityCreateIn<out TInJson> : IDataverseTransactableIn<TInJson>
    where TInJson : notnull
{
    string EntityPluralName { get; }

    FlatArray<string> SelectFields { get; init; }

    TInJson EntityData { get; }

    FlatArray<DataverseExpandedField> ExpandFields { get; init; }

    bool? SuppressDuplicateDetection { get; init; }

    TInJson? IDataverseTransactableIn<TInJson>.Entity
        =>
        EntityData;
}
