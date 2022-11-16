using System.Collections.Generic;

namespace GGroupp.Infra;

public readonly record struct DataverseEntitySetGetOut<TEntityJson>
{
    public DataverseEntitySetGetOut(FlatArray<TEntityJson> value)
        =>
        Value = value;

    public FlatArray<TEntityJson> Value { get; }
}